using Rankomizer.Domain.Catalog;
using TMDbLib.Objects.Movies;

namespace Rankomizer.Application.Catalog;

public interface ICatalogRepository
{
    Task<List<MovieItem>> GetAllMovies();
    Task<List<object>> GetAllItemsAsync();
    Task<List<object>> LoadFullItemsAsync(List<(Guid ItemId, string ItemType)> items);
}