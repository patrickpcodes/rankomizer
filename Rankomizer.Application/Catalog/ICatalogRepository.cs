using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Catalog;

public interface ICatalogRepository
{
    Task<List<CatalogEntry>> GetAllItemsAsync();
    Task<CatalogEntry?> GetItemByIdAsync(Guid id);
    Task<T?> GetTypedItemDataAsync<T>(Guid itemId);
}