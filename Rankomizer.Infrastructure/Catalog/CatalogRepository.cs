using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Catalog;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Infrastructure.Catalog;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _context;

    public CatalogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CatalogEntry>> GetAllItemsAsync()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<CatalogEntry?> GetItemByIdAsync(Guid id)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id);
    }

    public async Task<T> GetTypedItemDataAsync<T>(Guid itemId)
    {
        var item = await GetItemByIdAsync(itemId);
        if(item != null)
            return item.GetDataAs<T>();
        return default!;
    }
}