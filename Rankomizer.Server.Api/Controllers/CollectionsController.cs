using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.DTOs;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Server.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class CollectionsController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CollectionsController( ApplicationDbContext context )
    {
        _context = context;
    }
    
    [HttpGet("collections/{collectionId}")]
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