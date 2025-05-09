using Rankomizer.Application.Tmdb;
using Rankomizer.Server.Api.Seeding;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Movies;

namespace Rankomizer.Tests.Integration.Seeding;

public class SeedingTmdbManager : ITmdbManager
{
    public async Task<Collection> GetTmdbCollectionAsync( int collectionId )
    {
        return await FileSeeding.ReadJson<Collection>(FileSeeding.TmdbCollection, $"{collectionId}");
        
    }

    public async Task<Movie> GetTmdbMovieAsync( int movieId )
    {
        return await FileSeeding.ReadJson<Movie>(FileSeeding.TmdbMovie, $"{movieId}");
    }
}