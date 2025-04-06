using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Catalog;
using SpotifyAPI.Web;

namespace Rankomizer.Server.Api.Endpoints.Catalog;

internal sealed class TestGet : IEndpoint
{
    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapGet( "/api/catalog", [AllowAnonymous] async ( ICatalogRepository repo ) =>
        {
            var items = await repo.GetAllItemsAsync();
            return Results.Ok( items );
        } );
        app.MapGet( "/api/movie", [AllowAnonymous] async ( ICatalogRepository repo ) =>
        {
            var movieItems = await repo.GetAllMovies();
            return Results.Ok( movieItems );
        } );
        app.Map( "/api/authorized", [Authorize] async () => { return Results.Ok( new { message = "Authorized" } ); } );
        app.Map( "/api/songs", [AllowAnonymous] async ( IConfiguration configuration ) =>
        {
            var config = SpotifyClientConfig
                         .CreateDefault()
                         .WithAuthenticator( new ClientCredentialsAuthenticator( configuration["Spotify:ClientId"],
                             configuration["Spotify:ClientSecret"] ) );

            var spotify = new SpotifyClient( config );
            var searchResponse =
                await spotify.Search.Item( new SearchRequest( SearchRequest.Types.Artist, "Bruce Springsteen" ) );

            var track = await spotify.Tracks.Get( "1s6ux0lNiTziSrd7iUAADH" );
            Console.WriteLine( track.Name );
        } );
    }
}