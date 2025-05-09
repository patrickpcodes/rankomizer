using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Collections.CreateCollection;

internal sealed class CreateCollectionCommandHandler(
    ICollectionRepository repository
) : ICommandHandler<CreateCollectionCommand, Guid>
{
    public async Task<Result<Guid>> Handle( CreateCollectionCommand command, CancellationToken cancellationToken )
    {
        var createdCollection = await repository.CreateCollection( command.Name, command.Description, command.ImageUrl, command.UserId );
        if(createdCollection.IsFailure)
            return createdCollection.Error;
        
        return createdCollection.Value; 
    }
}