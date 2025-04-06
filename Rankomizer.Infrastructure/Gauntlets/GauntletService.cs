using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.DTOs;
using Rankomizer.Application.Gauntlet;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Infrastructure.Gauntlets;

public class GauntletService : IGauntletService
{
    private readonly ApplicationDbContext _context;

    public GauntletService( ApplicationDbContext context )
    {
        _context = context;
    }

    public async Task<Duel?> GetNextPendingDuelAsync( Guid gauntletId )
    {
        return await _context.Duels
                             .AsNoTracking()
                             .Include( d => d.RosterItemA ).ThenInclude( ri => ri.Item )
                             .Include( d => d.RosterItemB ).ThenInclude( ri => ri.Item )
                             .Include( d => d.Gauntlet )
                             .ThenInclude( g => g.RosterItems )
                             .ThenInclude( ri => ri.Item )
                             .Where( d => d.GauntletId == gauntletId && d.WinnerRosterItemId == null )
                             .OrderBy( d => d.Id ) // or your hash order
                             .FirstOrDefaultAsync();

        return await _context.Duels
                             .Include( d => d.Gauntlet )
                             .Include( d => d.RosterItemA ).ThenInclude( ri => ri.Item )
                             .Include( d => d.RosterItemB ).ThenInclude( ri => ri.Item )
                             .Where( d => d.GauntletId == gauntletId && d.WinnerRosterItemId == null )
                             .OrderBy( d => d.Id )
                             .FirstOrDefaultAsync();
    }
    public async Task<List<RosterItem>> GetRosterAsync(Guid gauntletId)
    {
        var rosterItems = await _context.RosterItems
                                   .Where(ri => ri.GauntletId == gauntletId)
                                   .Include(ri => ri.Item)
                                   .ToListAsync();

        return rosterItems;
    }
    public async Task<Duel?> SubmitDuelResultAsync( Guid duelId, Guid winnerRosterItemId )
    {
        var duel = await _context.Duels
                                 .Include( d => d.RosterItemA ).ThenInclude( ri => ri.Item )
                                 .Include( d => d.RosterItemB ).ThenInclude( ri => ri.Item )
                                 .Include( d => d.Gauntlet )
                                 .ThenInclude( g => g.RosterItems )
                                 .ThenInclude( ri => ri.Item )
                                 .FirstOrDefaultAsync( d => d.Id == duelId );

        if ( duel == null || duel.WinnerRosterItemId != null )
            return null;

        duel.WinnerRosterItemId = winnerRosterItemId;

        var loser = duel.RosterItemAId == winnerRosterItemId
            ? duel.RosterItemB
            : duel.RosterItemA;

        var winner = duel.RosterItemAId == winnerRosterItemId
            ? duel.RosterItemA
            : duel.RosterItemB;

        winner.Wins += 1;
        loser.Losses += 1;

        
        const double K = 32.0;
        // Elo rating update
        var Ra = winner.Score;
        var Rb = loser.Score;

        double Ea = 1.0 / (1.0 + Math.Pow(10, (Rb - Ra) / 400.0));
        double Eb = 1.0 / (1.0 + Math.Pow(10, (Ra - Rb) / 400.0));

        winner.Score = Math.Round(Ra + K * (1 - Ea), 2);
        loser.Score  = Math.Round(Rb + K * (0 - Eb), 2);
        
        await _context.SaveChangesAsync();

        // return the next duel
        return await GetNextPendingDuelAsync( duel.GauntletId );
    }

    public async Task<Gauntlet> CreateGauntletFromCollectionAsync( Guid collectionId, Guid userId,
                                                                   string? gauntletName = null )
    {
        // Load the collection with its items and base Item
        var collection = await _context.Collections
                                       .Include( c => c.Items )
                                       .ThenInclude( ci => ci.Item )
                                       .FirstOrDefaultAsync( c => c.Id == collectionId );

        if ( collection == null )
            throw new Exception( "Collection not found." );

        // Create the gauntlet
        var gauntlet = new Gauntlet
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CollectionId = collection.Id,
            Name = gauntletName ?? $"Gauntlet - {collection.Name}",
            CreatedAt = DateTime.UtcNow,
            RosterItems = new List<RosterItem>(),
            Duels = new List<Duel>()
        };

        // Create RosterItems (linking Items into this Gauntlet)
        var rosterItems = collection.Items.Select( ci => new RosterItem
        {
            Id = Guid.NewGuid(),
            Gauntlet = gauntlet,
            ItemId = ci.ItemId,
            Item = ci.Item,
            
            Wins = 0,
            Losses = 0,
            Score = 1600.0,
            Status = RosterItemStatus.Active
        } ).ToList();

        gauntlet.RosterItems = rosterItems;
        var duelsList = new List<Duel>();
        // Generate Duels (unordered pairs)
        for ( int i = 0; i < rosterItems.Count; i++ )
        {
            for ( int j = i + 1; j < rosterItems.Count; j++ )
            {
                var duel = new Duel
                {
                    Id = Guid.NewGuid(),
                    Gauntlet = gauntlet,
                    RosterItemA = rosterItems[i],
                    RosterItemB = rosterItems[j],
                    CreatedAt = DateTime.UtcNow
                };
                duelsList.Add( duel );
            }
        }

        Random rng = new Random();
        var shuffledList = duelsList.OrderBy( x => rng.Next() ).ToList();
        _context.Duels.AddRange( shuffledList );
        _context.Gauntlets.Add( gauntlet );
        await _context.SaveChangesAsync();

        return gauntlet;
    }
}