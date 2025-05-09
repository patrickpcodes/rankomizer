using Microsoft.Extensions.Configuration;
using Rankomizer.Application.Tmdb;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Movies;

namespace Rankomizer.Infrastructure.Tmdb;

public class TmdbManager : ITmdbManager
{
    private readonly TMDbClient _client;
    public TmdbManager(IConfiguration configuration)
    {
        var tmdbApiKey = configuration["Tmdb:ApiKey"];
        _client = new TMDbClient( tmdbApiKey );
    }
    
    public async Task<Collection> GetTmdbCollectionAsync(int collectionId)
    {
        var collection = await _client.GetCollectionAsync(collectionId);
        return collection;
    }
    public async Task<Movie> GetTmdbMovieAsync(int movieId)
    {
        var movie = await _client.GetMovieAsync(movieId);
        return movie;
    }
}