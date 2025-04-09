using System.Text.Json;
using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.DTOs;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api;

namespace Rankomizer.Tests.Integration.Collections;

public class CreateCollectionTests( IntegrationTestFactory<Program, ApplicationDbContext> factory )
    : IClassFixture<IntegrationTestFactory<Program, ApplicationDbContext>>
{
    [Fact]
    public async Task CollectionTest_CreateCollection_EnsureCollectionIdIsReturnedAndValid()
    {
        var (client, rankomizerJwt) = await AuthenticationManager.LoginAndGetCookie( factory, SeededUsers.PowerUser );

        var newCollection = new Collection()
        {
            Name = "Test1",
            Description = "",
            ImageUrl = "",
        };
        var collectionDto = await CollectionManager.CreateCollectionAsync( client, newCollection, rankomizerJwt );
        Assert.Equal( newCollection.Name, collectionDto.Name );
    }
}