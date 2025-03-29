using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Catalog;

namespace Rankomizer.Server.Api.Endpoints.Catalog;

internal sealed class TestGet : IEndpoint
{
    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapGet( "/api/catalog", [AllowAnonymous]async ( ICatalogRepository repo ) =>
        {
            var items = await repo.GetAllItemsAsync();
            return Results.Ok( items );
        } );
    }
}