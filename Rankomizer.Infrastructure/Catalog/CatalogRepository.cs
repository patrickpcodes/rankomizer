using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rankomizer.Application.Catalog;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;
using TMDbLib.Objects.Movies;
using Movie = Rankomizer.Domain.Catalog.Movie;

namespace Rankomizer.Infrastructure.Catalog;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<object>> LoadFullItemsAsync(List<(Guid ItemId, string ItemType)> items)
    {
        var result = new List<object>();

        var movieIds = items.Where(i => i.ItemType == "Movie").Select(i => i.ItemId).ToList();
        var songIds = items.Where(i => i.ItemType == "Song").Select(i => i.ItemId).ToList();
        var paintingIds = items.Where(i => i.ItemType == "Painting").Select(i => i.ItemId).ToList();

        if (movieIds.Any())
            result.AddRange(await _context.Movies.Where(m => movieIds.Contains(m.ItemId)).ToListAsync());

        if (songIds.Any())
            result.AddRange(await _context.Songs.Where(s => songIds.Contains(s.ItemId)).ToListAsync());

        if (paintingIds.Any())
            result.AddRange(await _context.Paintings.Where(p => paintingIds.Contains(p.ItemId)).ToListAsync());

        return result;
    }

    public async Task<List<Movie>> GetAllMovies()
    {
       var movieList = await _context.Movies.ToListAsync();

       foreach ( var movie in movieList )
       {
          TMDbLib.Objects.Movies.Movie tempMovie = movie.SourceJson.RootElement.Deserialize<TMDbLib.Objects.Movies.Movie>(new JsonSerializerOptions
          {
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
              IncludeFields = true,
              DefaultIgnoreCondition = JsonIgnoreCondition.Never
          }); 
          Console.WriteLine(tempMovie.Title);
       }
       return movieList;
    }

    public async Task<List<object>> GetAllItemsAsync()
    {
        var result = new List<object>();

        result.AddRange(await _context.Movies.ToListAsync());
        result.AddRange(await _context.Songs.ToListAsync());
        result.AddRange(await _context.Paintings.ToListAsync());

        return result;
    }
}