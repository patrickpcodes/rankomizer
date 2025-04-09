using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api;
using Rankomizer.Server.Api.Seeding;

namespace Rankomizer.Tests.Integration;

public enum SeededUsers
{
    PowerUser,
    Admin,
    Guest
}
public static class SeededUser
{
    public static string GetUser( SeededUsers usersEnum ) =>
        usersEnum switch
        {
            SeededUsers.PowerUser => "poweruser",
            _ => throw new ArgumentOutOfRangeException( nameof(usersEnum), usersEnum, null ),
        };
}
public static class AuthenticationManager
{
    public static UserRecord GetUser( IntegrationTestFactory<Program, ApplicationDbContext> factory,
                                      SeededUsers seededUser)
    {
        var config = factory.Services.GetRequiredService<IConfiguration>();
        var user = config.GetSection( SeededUser.GetUser( seededUser ) ).Get<UserRecord>();

        return user;
    }

    public static async Task<(HttpClient client, string cookie)> LoginAndGetCookie(
        IntegrationTestFactory<Program, ApplicationDbContext> factory, SeededUsers usersEnum )
    {
        var config = factory.Services.GetRequiredService<IConfiguration>();
        var user = config.GetSection( SeededUser.GetUser( usersEnum ) ).Get<UserRecord>();

        return await LoginAndGetCookie( factory, user.Email, user.Password );
    }

    public static async Task<(HttpClient client, string cookie)> LoginAndGetCookie(
        IntegrationTestFactory<Program, ApplicationDbContext> factory, string email, string password )
    {
        var loginData = new { email = email, password = password };
        var client = factory.CreateClient();
        // Act: Call the login endpoint
        var loginResponse = await client.PostAsJsonAsync( "/api/users/login", loginData );
        loginResponse.EnsureSuccessStatusCode();

        var rankomizerJwt = "";
        var setCookieHeader = loginResponse.Headers.GetValues( "Set-Cookie" ).FirstOrDefault();
        if ( setCookieHeader != null )
        {
            // Split by ';' to isolate the key-value pair.
            var cookieParts = setCookieHeader.Split( ';' );
            // The first part should be "rankomizer-jwt=your_jwt_value"
            var keyValue = cookieParts[0].Split( '=' );
            if ( keyValue.Length == 2 && keyValue[0].Trim() == "rankomizer-jwt" )
            {
                rankomizerJwt = keyValue[1].Trim();
            }
            else
            {
                Assert.Fail( "There was an error parsing the Set-Cookie header." );
            }
        }

        return ( client, rankomizerJwt );
    }
}