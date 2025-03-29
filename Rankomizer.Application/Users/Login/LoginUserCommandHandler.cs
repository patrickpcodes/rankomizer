using Microsoft.AspNetCore.Identity;
using Rankomizer.Application.Abstractions.Authentication;
using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;

namespace Rankomizer.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITokenProvider tokenProvider
) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle( LoginUserCommand command, CancellationToken cancellationToken )
    {
        var user = await userManager.FindByEmailAsync( command.Email );
        if ( user is null )
        {
            return ApplicationUserErrors.NotFoundByEmail( command.Email );
        }

        var result = await userManager.CheckPasswordAsync( user, command.Password );
        if ( !result )
            return ApplicationUserErrors.Unauthorized();
        
        var token = tokenProvider.Create( user );
        return token;


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