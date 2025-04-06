using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.DTOs;
using Rankomizer.Application.Gauntlet;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Server.Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
// [Authorize]
public class GauntletController : ControllerBase
{
    private readonly IGauntletService _gauntletService;
    private readonly ApplicationDbContext _context;

    public GauntletController( IGauntletService gauntletService, ApplicationDbContext context )
    {
        _gauntletService = gauntletService;
        _context = context;
    }

    [HttpGet( "{gauntletId}" )]
    public async Task<IActionResult> Get( Guid gauntletId )
    {
        var gauntlet = await _context.Gauntlets
                                     .Include( g => g.RosterItems )
                                     .ThenInclude( ri => ri.Item )
                                     .FirstOrDefaultAsync( g => g.Id == gauntletId );

        if ( gauntlet == null )
            return NotFound();

        var dto = new GauntletDto
        {
            Id = gauntlet.Id,
            Name = gauntlet.Name,
            Description = gauntlet.Description,
            Roster = gauntlet.RosterItems.Select( ri =>
            {
                var item = ri.Item;
                var rosterDto = new RosterItemDto
                {
                    Id = ri.Id,
                    Status = ri.Status,
                    Wins = ri.Wins,
                    Losses = ri.Losses,
                    Score = ri.Score,
                    ItemType = item.ItemType,
                    Name = item.Name,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl
                };

                // Concrete type-specific details
                switch ( item )
                {
                    case Movie movie:
                        rosterDto.Details = new MovieDetailsDto
                        {
                            TmdbId = movie.TmdbId,
                            ImdbId = movie.ImdbId,
                            ReleaseDate = movie.ReleaseDate,
                            SourceJson = movie.SourceJson,
                        };
                        break;
                    // case Song song:
                    //     rosterDto.Details = new SongDetailsDto
                    //     {
                    //         Artist = song.Artist,
                    //         Album = song.Album,
                    //         DurationSeconds = song.DurationSeconds
                    //     };
                    //     break;
                    // add more as needed
                }

                return rosterDto;
            } ).ToList()
        };

        return Ok( dto );
    }


    [Authorize]
    [HttpPost( "create" )]
    public async Task<ActionResult<Gauntlet>> CreateGauntlet( [FromBody] CreateGauntletRequest request )
    {
        // Simulate user auth for now — replace with actual UserId
        var userIdClaim = User.FindFirst( ClaimTypes.NameIdentifier )?.Value;
        if ( userIdClaim == null )
            return Unauthorized();

        var userId = Guid.Parse( userIdClaim ); // or int, whatever your ID type is


        try
        {
            var gauntlet = await _gauntletService.CreateGauntletFromCollectionAsync(
                request.CollectionId,
                userId,
                request.GauntletName );

            return Ok( new {gauntletId= gauntlet.Id} );
        }
        catch ( Exception ex )
        {
            return BadRequest( new { message = ex.Message } );
        }
    }
    
    [HttpPost("{gauntletId}/start")]
    public async Task<ActionResult<DuelDto?>> StartGauntlet(Guid gauntletId)
    {
        var duel = await _gauntletService.GetNextPendingDuelAsync(gauntletId);
        if (duel == null)
        {
            var roster = await _gauntletService.GetRosterAsync(gauntletId);
            var rosterDto = roster
                            .OrderByDescending(ri => ri.Score)
                            .ThenBy(ri => ri.Item.Name)
                            .Select(ri => ri.ToDto())
                            .ToList();
            return Ok(new DuelResponseDto { Duel = null, Roster = rosterDto });
        }

        return Ok(new DuelResponseDto
        {
            Duel = duel.ToDto(),
            Roster = duel.Gauntlet.RosterItems.Select(ri => ri.ToDto()).ToList()
        });
    }

    [HttpPost("duel/submit")]
    public async Task<ActionResult<DuelDto?>> SubmitDuel([FromBody] SubmitDuelRequest request)
    {
        var nextDuel = await _gauntletService.SubmitDuelResultAsync(
            request.DuelId,
            request.WinnerRosterItemId
        );

        if (nextDuel == null)
        {
            var duel = await _context.Duels
                                .Include(d => d.Gauntlet)
                                .ThenInclude(g => g.RosterItems)
                                .ThenInclude(ri => ri.Item)
                                .FirstOrDefaultAsync(d => d.Id == request.DuelId);

            var roster = duel?.Gauntlet?.RosterItems.Select(ri => ri.ToDto()).ToList() ?? new();

            return Ok(new DuelResponseDto { Duel = null, Roster = roster });
        }

        return Ok(new DuelResponseDto
        {
            Duel = nextDuel.ToDto(),
            Roster = nextDuel.Gauntlet.RosterItems.Select(ri => ri.ToDto()).ToList()
        });
    }
}

public class CreateGauntletRequest
{
    public Guid CollectionId { get; set; }
    public string? GauntletName { get; set; }
}

public static class DuelExtensions
{
    public static DuelDto ToDto(this Duel duel)
    {
        var gauntlet = duel.Gauntlet;

        return new DuelDto
        {
            DuelId = duel.Id,
            OptionA = duel.RosterItemA.ToDto(),
            OptionB = duel.RosterItemB.ToDto(),

        };
    }

    public static RosterItemDto ToDto(this RosterItem ri)
    {
        var item = ri.Item;

        return new RosterItemDto
        {
            Id = ri.Id,
            Status = ri.Status,
            Wins = ri.Wins,
            Losses = ri.Losses,
            Score = ri.Score,
            ItemType = item.ItemType,
            Name = item.Name,
            Description = item.Description,
            ImageUrl = item.ImageUrl,
            Details = item switch
            {
                Movie m => new MovieDetailsDto
                {
                    TmdbId = m.TmdbId,
                    ImdbId = m.ImdbId,
                    ReleaseDate = m.ReleaseDate,
                    // SourceJson = m.SourceJson,
                },
                // Song s => new SongDetailsDto
                // {
                //     Artist = s.Artist,
                //     Album = s.Album,
                //     DurationSeconds = s.DurationSeconds
                // },
                _ => null
            }
        };
    }
}
