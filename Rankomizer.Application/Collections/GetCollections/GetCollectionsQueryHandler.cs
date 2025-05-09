using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Application.Collections.GetCollectionById;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Application.Collections.GetCollections;


internal sealed class GetCollectionsQueryHandler : IQueryHandler<GetCollectionsQuery, List<CollectionDto>>
{
    private readonly ICollectionRepository _repository;
    public GetCollectionsQueryHandler( ICollectionRepository repository )
    {
        _repository = repository;
    }
    
    public async Task<Result<List<CollectionDto>>> Handle( GetCollectionsQuery request, CancellationToken cancellationToken )
    {
        var collections = await _repository.GetCollections( request.userId );
        if(collections.IsFailure)
            return collections.Error;

        return collections.Value.Select(c => c.ToDto()).ToList();
    }
}