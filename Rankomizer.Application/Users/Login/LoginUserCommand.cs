using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;