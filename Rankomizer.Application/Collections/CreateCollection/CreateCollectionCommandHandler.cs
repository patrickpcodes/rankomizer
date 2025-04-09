using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Collections.CreateCollection;

internal sealed class CreateCollectionCommandHandler(
    ICollectionRepository repository
) : ICommandHandler<CreateCollectionCommand, Collection>
{
    public async Task<Result<Collection>> Handle( CreateCollectionCommand command, CancellationToken cancellationToken )
    {
        var createdCollection = await repository.CreateCollection( command.Name, command.Description, command.ImageUrl, command.UserId );
        
        
        return createdCollection; 


        // User? user = await context.Users
        //                           .AsNoTracking()
        //                           .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);
        //
        // if (user is null)
        // {
        //     return Result.Failure<string>(UserErrors.NotFoundByEmail);
        // }
        //
        // bool verified = passwordHasher.Verify(command.Password, user.PasswordHash);
        //
        // if (!verified)
        // {
        //     return Result.Failure<string>(UserErrors.NotFoundByEmail);
        // }
        //
        // string token = tokenProvider.Create(user);
        //
        // return token;
    }
}