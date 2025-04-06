using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api;
using Rankomizer.Server.Api.Seeding;

namespace Rankomizer.Tests.Integration;

public class Tests : IClassFixture<IntegrationTestFactory<Program, ApplicationDbContext>>
{
    private readonly IntegrationTestFactory<Program, ApplicationDbContext> _factory;
    public Tests( IntegrationTestFactory<Program, ApplicationDbContext> factory )
    {
        _factory = factory;
    }

    [Fact]
    public async Task Foo()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync( "/WeatherForecast" );

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Bar()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync( "/api/movie" );
        var text = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task Testing(){
      //Assert.Pass();
      Console.WriteLine("I am in this test");

    }
    [Fact]
    public async Task Baz()
    {
        var config = _factory.Services.GetRequiredService<IConfiguration>();
        var user = config.GetSection( "poweruser" ).Get<UserRecord>();
        
        var loginData = new { email = user.Email, password = user.Password };

        var client = _factory.CreateClient();
        // Act: Call the login endpoint
        var loginResponse = await client.PostAsJsonAsync("/users/login", loginData);
        loginResponse.EnsureSuccessStatusCode(); 
        var text = await loginResponse.Content.ReadAsStringAsync();
        var rankomizerJwt = "";
        var setCookieHeader = loginResponse.Headers.GetValues("Set-Cookie").FirstOrDefault();
        if (setCookieHeader != null)
        {
            // Split by ';' to isolate the key-value pair.
            var cookieParts = setCookieHeader.Split(';');
            // The first part should be "rankomizer-jwt=your_jwt_value"
            var keyValue = cookieParts[0].Split('=');
            if (keyValue.Length == 2 && keyValue[0].Trim() == "rankomizer-jwt")
            {
                rankomizerJwt = keyValue[1].Trim();
            }
            else
            {
                Assert.Fail("There was an error parsing the Set-Cookie header.");
            }
        }
        
        // Use the JWT cookie in a future call by manually adding it to the request header
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/authorized");
        request.Headers.Add("Cookie", $"rankomizer-jwt={rankomizerJwt}");

        var authorizedResponse = await client.SendAsync(request);
        authorizedResponse.EnsureSuccessStatusCode();
        var result = await authorizedResponse.Content.ReadAsStringAsync();
        
    }
}
