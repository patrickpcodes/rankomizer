using System.Text.Json.Serialization;

namespace Rankomizer.Application.Users.GetLoggedInUser;

public sealed class UserDetailsResponse
{
    [JsonPropertyName( "id" )]
    public Guid Id { get; init; }

    [JsonPropertyName( "email" )]
    public string Email { get; init; }

    [JsonPropertyName( "username" )]
    public string Username { get; set; }
}