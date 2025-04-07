using Microsoft.Extensions.Logging;
using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Application.Users.LogoutUser;

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