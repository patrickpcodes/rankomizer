
using Microsoft.EntityFrameworkCore;
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
    public async Task<Gauntlet> CreateGauntletFromCollectionAsync(Guid collectionId, Guid userId, string? gauntletName = null)
    {
        // Load the collection with its items and base Item
        var collection = await _context.Collections
                                       .Include(c => c.Items)
                                       .ThenInclude(ci => ci.Item)
                                       .FirstOrDefaultAsync(c => c.Id == collectionId);

        if (collection == null)
            throw new Exception("Collection not found.");

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
        var rosterItems = collection.Items.Select(ci => new RosterItem
        {
            Id = Guid.NewGuid(),
            Gauntlet = gauntlet,
            ItemId = ci.ItemId,
            Item = ci.Item
        }).ToList();

        gauntlet.RosterItems = rosterItems;

        // Generate Duels (unordered pairs)
        for (int i = 0; i < rosterItems.Count; i++)
        {
            for (int j = i + 1; j < rosterItems.Count; j++)
            {
                var duel = new Duel
                {
                    Id = Guid.NewGuid(),
                    Gauntlet = gauntlet,
                    RosterItemA = rosterItems[i],
                    RosterItemB = rosterItems[j],
                    CreatedAt = DateTime.UtcNow
                };

                gauntlet.Duels.Add(duel);
            }
        }

        _context.Gauntlets.Add(gauntlet);
        await _context.SaveChangesAsync();

        return gauntlet;
    }

}