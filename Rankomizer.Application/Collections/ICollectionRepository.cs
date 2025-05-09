using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Collections;

public interface ICollectionRepository
{
    Task<Result<Guid>> CreateCollection( string name, string description, string imageUrl, Guid userId );


    Task<Result<Collection>> GetCollectionById(Guid collectionId);
    Task<Result<List<Collection>>> GetCollections(Guid userId);
}