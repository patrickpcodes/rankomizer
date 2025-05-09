using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Movies;

namespace Rankomizer.Application.Tmdb;

public interface ITmdbManager
{
    Task<Collection> GetTmdbCollectionAsync(int collectionId);
    Task<Movie> GetTmdbMovieAsync(int movieId);
}