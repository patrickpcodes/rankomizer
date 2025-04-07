using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.User;

namespace Rankomizer.Application.Users.LogoutUser;

public record LogoutUserCommand( ApplicationUser User ) : ICommand<bool>;
