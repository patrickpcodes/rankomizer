using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Collections;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Infrastructure.Collections;

public class CollectionRepository : ICollectionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CollectionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> CreateCollection(string name, string description, string imageUrl, Guid userId)
    {
        var collection = new Collection
        {
            Name = name,
            Description = description,
            ImageUrl = imageUrl,
            CreatedByUserId = userId
        };
        _dbContext.Collections.Add(collection);
        await _dbContext.SaveChangesAsync();
        //TODO Check for error 
       return collection.Id;
       

    }
    public async Task<Result<List<Collection>>> GetCollections(Guid userId)
    {
        var collections = await _dbContext.Collections
                                         .Include( c => c.Items )
                                         .ThenInclude( ci => ci.Item )
                                         // .Where(d => d.CreatedByUserId == userId) 
                                         .ToListAsync();

        if (collections == null)
            return new Error("Collection not found", "ddd", ErrorType.NotFound);

        return collections;
    }
    public async Task<Result<Collection>> GetCollectionById(Guid collectionId)
    {
        var collection = await _dbContext.Collections
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Item)
            .FirstOrDefaultAsync(c => c.Id == collectionId);

        if (collection == null)
            return new Error("Collection not found", "ddd", ErrorType.NotFound);

        return collection;
    }
}