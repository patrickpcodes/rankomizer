using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rankomizer.Application.Users.GetLoggedInUser;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.DTOs;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api;
using Rankomizer.Server.Api.Infrastructure;
using Rankomizer.Server.Api.Seeding;

namespace Rankomizer.Tests.Integration.User;

public class LoginUserTests : IClassFixture<IntegrationTestFactory<Program, ApplicationDbContext>>
{
    private readonly IntegrationTestFactory<Program, ApplicationDbContext> _factory;

    public LoginUserTests( IntegrationTestFactory<Program, ApplicationDbContext> factory )
    {
        _factory = factory;
    }

    [Fact]
    public async Task LoginUserTests_LoginWithInvalidEmail_ShouldHaveNotFoundResponse()
    {
        var email = "abcde@klmno.com";
        var loginData = new { email, password = "fghij" };

        var client = _factory.CreateClient();
        // Act: Call the login endpoint
        var loginResponse = await client.PostAsJsonAsync( "/api/users/login", loginData );
        Assert.Equal( HttpStatusCode.NotFound, loginResponse.StatusCode );
        var actualProblemDetail =
            JsonSerializer.Deserialize<ProblemDetails>( await loginResponse.Content.ReadAsStringAsync() );
        if ( actualProblemDetail == null )
            Assert.Fail( "Error Parsing actualProblemDetails" );

        var expectedUserError = ApplicationUserErrors.NotFoundByEmail( email );
        var expectedProblemHttpResult =
            CustomResults.Problem( Result.Failure( expectedUserError ) ) as ProblemHttpResult;
        var expectedProblemDetail = expectedProblemHttpResult.ProblemDetails;
        if ( expectedProblemDetail == null )
            Assert.Fail( "Error Parsing expectedProblemDetails" );

        Assert.True( actualProblemDetail.IsSameAs( expectedProblemDetail ) );
    }

    [Fact]
    public async Task LoginUserTests_LoginWithValidEmailAndInvalidPassword_ShouldHaveUnauthorizedResponse()
    {
        var user = AuthenticationManager.GetUser( _factory, SeededUsers.PowerUser );

        var loginData = new { email = user.Email, password = user.Password + "ERROR" };

        var client = _factory.CreateClient();
        // Act: Call the login endpoint
        var loginResponse = await client.PostAsJsonAsync( "/api/users/login", loginData );
        Assert.Equal( HttpStatusCode.Unauthorized, loginResponse.StatusCode );
        var actualProblemDetail =
            JsonSerializer.Deserialize<ProblemDetails>( await loginResponse.Content.ReadAsStringAsync() );
        if ( actualProblemDetail == null )
            Assert.Fail( "Error Parsing actualProblemDetails" );

        var expectedUserError = ApplicationUserErrors.Unauthorized();
        var expectedProblemHttpResult =
            CustomResults.Problem( Result.Failure( expectedUserError ) ) as ProblemHttpResult;
        var expectedProblemDetail = expectedProblemHttpResult.ProblemDetails;
        if ( expectedProblemDetail == null )
            Assert.Fail( "Error Parsing expectedProblemDetails" );

        Assert.True( actualProblemDetail.IsSameAs( expectedProblemDetail ) );
    }

    [Fact]
    public async Task LoginUserTests_LoginWithAuthorizedUser_ValidateUserDetails()
    {
        var (client, rankomizerJwt) = await AuthenticationManager.LoginAndGetCookie( _factory, SeededUsers.PowerUser );
        var userRecord = AuthenticationManager.GetUser( _factory, SeededUsers.PowerUser );
        // Use the JWT cookie in a future call by manually adding it to the request header
        var request = new HttpRequestMessage( HttpMethod.Get, "/api/users/details" );
        request.Headers.Add( "Cookie", $"rankomizer-jwt={rankomizerJwt}" );

        var authorizedResponse = await client.SendAsync( request );
        authorizedResponse.EnsureSuccessStatusCode();
        var result = await authorizedResponse.Content.ReadAsStringAsync();
        var userDetails = JsonSerializer.Deserialize<UserDetailsResponse>( result );
        
        Assert.Equal( userRecord.Email, userDetails.Email );
        Assert.Equal( userRecord.Username, userDetails.Username );

        var logoutRequest = new HttpRequestMessage( HttpMethod.Post, "/api/users/logout" );
        logoutRequest.Headers.Add( "Cookie", $"rankomizer-jwt={rankomizerJwt}" );
        var logoutResponse = await client.SendAsync( logoutRequest );
        logoutResponse.EnsureSuccessStatusCode();
    }
}