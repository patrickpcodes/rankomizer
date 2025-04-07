using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rankomizer.Application.Users.GetLoggedInUser;
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
    
}
