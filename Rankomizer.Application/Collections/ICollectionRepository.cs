using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Collections;

public interface ICollectionRepository
{
    Task<Result<Collection>> CreateCollection( string name, string description, string imageUrl, Guid userId );
}