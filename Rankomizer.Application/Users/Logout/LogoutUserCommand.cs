using MediatR;
using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;

namespace Rankomizer.Application.Users.Logout;

public record LogoutUserCommand( ApplicationUser User ) : ICommand<bool>;
