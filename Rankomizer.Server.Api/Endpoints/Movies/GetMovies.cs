using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Catalog;

namespace Rankomizer.Server.Api.Endpoints.Movies;

internal sealed class GetMovies : IEndpoint
{
    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapGet( "/api/movies", [AllowAnonymous] async ( ICatalogRepository repo ) =>
        {
            var movieItems = await repo.GetAllMovies();
            return Results.Ok( movieItems );
        } )
        .WithTags(Tags.Movies);
    }
}