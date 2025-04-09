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

    public async Task<Result<Collection>> CreateCollection(string name, string description, string imageUrl, Guid userId)
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
       return collection;
       

    }
}