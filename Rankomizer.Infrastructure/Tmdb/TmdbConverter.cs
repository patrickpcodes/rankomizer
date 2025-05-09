using System.Text.Json;
using Rankomizer.Domain.Catalog;
using TMDbLib.Client;
using Rankomizer.Infrastructure.Json;
namespace Rankomizer.Infrastructure.Tmdb;

public class TmdbConverter
{
    public static Collection GetCollectionFromTmdbCollection( TMDbLib.Objects.Collections.Collection tmdbCollection )
    {
        var collection = new Collection()
        {
            Name = "TMDB Collection: " + tmdbCollection.Name,
            Description = tmdbCollection.Overview,
            ImageUrl = tmdbCollection.PosterPath != null
                ? $"https://image.tmdb.org/t/p/w500{tmdbCollection.PosterPath}"
                : null,
            // CreatedByUserId = user.Id,
            // CreatedByUser = user,
            Items = new List<CollectionItem>()
        }; 
       return collection;
    }

    public static Movie GetMovieFromTmdbMovie( TMDbLib.Objects.Movies.Movie tmdbMovie )
    {
        var movie = new Movie()
        {
            ItemId = Guid.NewGuid(),
            Name = tmdbMovie.Title,
            Description = tmdbMovie.Overview,
            ImageUrl = tmdbMovie.PosterPath != null
                ? $"https://image.tmdb.org/t/p/w500{tmdbMovie.PosterPath}"
                : null,
            ItemType = ItemType.Movie,
            TmdbId = tmdbMovie.Id,
            ImdbId = tmdbMovie.ImdbId,
            ReleaseDate = tmdbMovie.ReleaseDate == null
                ? new DateTime()
                : DateTime.SpecifyKind( tmdbMovie.ReleaseDate.Value, DateTimeKind.Utc ),
            SourceJson = JsonDocument.Parse( JsonSerializer.Serialize( tmdbMovie, JsonOptions.CamelCase ) )
        };
        return movie;
    }
}