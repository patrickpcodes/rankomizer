using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Application.Collections.GetCollectionById;

internal sealed class GetCollectionByIdQueryHandler : IQueryHandler<GetCollectionByIdQuery, CollectionDto>
{
    private readonly ICollectionRepository _repository;
    public GetCollectionByIdQueryHandler( ICollectionRepository repository )
    {
        _repository = repository;
    }
    
    public async Task<Result<CollectionDto>> Handle( GetCollectionByIdQuery request, CancellationToken cancellationToken )
    {
        var collection = await _repository.GetCollectionById( request.collectionId );
        if(collection.IsFailure)
            return collection.Error;

        return collection.Value.ToDto();

    }
}