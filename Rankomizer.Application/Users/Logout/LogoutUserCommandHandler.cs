using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Rankomizer.Application.Abstractions.Authentication;
using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;

namespace Rankomizer.Application.Users.Logout;

internal sealed class LogoutUserCommandHandler(
    ILogger<LogoutUserCommandHandler> logger
) : ICommandHandler<LogoutUserCommand, bool>
{
    public async Task<Result<bool>> Handle( LogoutUserCommand command, CancellationToken cancellationToken )
    {
        var user = command.User;
        logger.LogInformation( $"Logged out user '{user.UserName}'" ); 
        return true;

    }
}