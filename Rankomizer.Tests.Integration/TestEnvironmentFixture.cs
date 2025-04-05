using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Rankomizer.Server.Api.Extensions;

namespace Rankomizer.Tests.Integration;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

public class IntegrationTestFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : DbContext
{
    public PostgreSqlContainer PostgresContainer { get; private set; }
    // public RedisContainer RedisContainer { get; private set; }

    public IntegrationTestFactory()
    {
        var postgresBuilder = new PostgreSqlBuilder()
                              .WithImage( "postgres:latest" )
                              .WithCleanUp( true )
                              .WithUsername( "postgres" )
                              .WithPassword( "postgres" )
                              .WithDatabase( "rankomizer_test" )
                              .WithPortBinding( 5432, true );


        var redisBuilder = new RedisBuilder()
                           .WithImage( "redis:latest" )
                           .WithCleanUp( true );

        PostgresContainer = postgresBuilder.Build();
        // RedisContainer = redisBuilder.Build();
    }

    protected override void ConfigureWebHost( IWebHostBuilder builder )
    {
        var temp = PostgresContainer.GetConnectionString();
        builder.ConfigureTestServices( services =>
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContext<TDbContext>( options =>
            {
                options.UseNpgsql( PostgresContainer.GetConnectionString() );
            } );
            // TODO add back in at a later point
            // services.EnsureDbCreated<TDbContext>();
        } );
    }

    public async Task InitializeAsync()
    {
        await PostgresContainer.StartAsync();
        // await RedisContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await PostgresContainer.StopAsync();
        // await RedisContainer.StopAsync();
    }
}