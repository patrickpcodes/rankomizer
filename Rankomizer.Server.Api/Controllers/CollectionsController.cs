using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.DTOs;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Server.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class CollectionsController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CollectionsController( ApplicationDbContext context )
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
    {
        var collections = await _context.Collections.AsNoTracking()
                                        // .Include( c => c.Items )
                                        // .ThenInclude( ci => ci.Item )
                                        .Where( d =>
                                            d.CreatedByUserId == Guid.Parse( "019609e6-13a1-7d1c-946d-c55523a3a5e7" ) )
                                        .ToListAsync();
        return Ok(collections);
    }
    
    [HttpGet("{collectionId}")]
    public async Task<ActionResult<CollectionDto>> GetCollectionWithItems(Guid collectionId)
    {
        var collection = await _context.Collections
                                       .Include(c => c.Items)
                                       .ThenInclude(ci => ci.Item)
                                       .FirstOrDefaultAsync(c => c.Id == collectionId);

        if (collection == null)
            return NotFound();

        var dto = new CollectionDto
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            ImageUrl = collection.ImageUrl,
            Items = collection.Items.Select(ci =>
            {
                var item = ci.Item;

                var itemDto = new ItemDto
                {
                    Id = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    ImageUrl = item.ImageUrl,
                    ItemType = item.ItemType
                };

                // Type-specific data
                switch (item)
                {
                    case Movie movie:
                        itemDto.Details = new MovieDetailsDto
                        {
                           TmdbId = movie.TmdbId,
                           ImdbId = movie.ImdbId,
                            ReleaseDate = movie.ReleaseDate,
                           SourceJson = movie.SourceJson,
                        };
                        break;

                    // case Song song:
                    //     itemDto.Details = new SongDetailsDto
                    //     {
                    //         Artist = song.Artist,
                    //         Album = song.Album,
                    //         DurationSeconds = song.DurationSeconds
                    //     };
                    //     break;

                    default:
                        itemDto.Details = null;
                        break;
                }

                return itemDto;
            }).ToList()
        };

        return Ok(dto);
    }

}