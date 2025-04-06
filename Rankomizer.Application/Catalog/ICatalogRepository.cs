using Rankomizer.Domain.Catalog;
using TMDbLib.Objects.Movies;
using Movie = Rankomizer.Domain.Catalog.Movie;

namespace Rankomizer.Application.Catalog;

public interface ICatalogRepository
{
    Task<List<Movie>> GetAllMovies();
    Task<List<object>> GetAllItemsAsync();
    Task<List<object>> LoadFullItemsAsync(List<(Guid ItemId, string ItemType)> items);
}