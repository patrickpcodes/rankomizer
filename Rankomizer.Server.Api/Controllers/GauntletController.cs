using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Gauntlet;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Infrastructure.DTOs;

namespace Rankomizer.Server.Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
[Authorize]
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

            return Ok( gauntlet );
        }
        catch ( Exception ex )
        {
            return BadRequest( new { message = ex.Message } );
        }
    }
}

public class CreateGauntletRequest
{
    public Guid CollectionId { get; set; }
    public string? GauntletName { get; set; }
}