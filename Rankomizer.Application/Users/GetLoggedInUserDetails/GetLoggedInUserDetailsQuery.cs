using Rankomizer.Application.Abstractions.Messaging;

namespace Rankomizer.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserDetailsQuery(Guid userId) : IQuery<UserDetailsResponse>;