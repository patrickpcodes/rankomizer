using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Domain.User;

public static class ApplicationUserErrors
{
    public const string UserNotFound = "Users.NotFound";
    public const string UserUnauthorized = "Users.Unauthorized";
    public const string UserNotFoundByEmail = "Users.NotFoundByEmail";
    public const string UserNotUnique = "Users.EmailNotUnique";
    
    public static Error NotFound(Guid userId) => Error.NotFound(
        UserNotFound,
        $"The user with the Id = '{userId}' was not found");

    public static Error Unauthorized() => Error.Unauthorized(
        UserUnauthorized,
        "You are not authorized to perform this action.");

    public static Error NotFoundByEmail(string email) => Error.NotFound(
        UserNotFoundByEmail,
        $"The user with the specified Email = '{email}' was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        UserNotUnique,
        "The provided email is not unique");
}