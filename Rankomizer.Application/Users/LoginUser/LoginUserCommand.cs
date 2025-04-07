using Rankomizer.Application.Abstractions.Messaging;

namespace Rankomizer.Application.Users.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;