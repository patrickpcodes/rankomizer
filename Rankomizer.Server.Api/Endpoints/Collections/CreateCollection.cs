using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Collections;
using Rankomizer.Application.Collections.CreateCollection;
using Rankomizer.Domain.DTOs;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api.Controllers;

namespace Rankomizer.Server.Api.Endpoints.Collections;

internal sealed class CreateCollection : IEndpoint
{
    public sealed record CreateCollectionRequest( string Name, string? Description, string? ImageUrl );

    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapPost( "/api/collections", [Authorize]
               async ( HttpContext context, CreateCollectionRequest request, ISender sender ) =>
               {
                   var userIdClaim = context.User.FindFirst( ClaimTypes.NameIdentifier )?.Value;
                   if ( userIdClaim == null )
                       return Results.Unauthorized();

                   var command = new CreateCollectionCommand(
                       request.Name,
                       request.Description,
                       request.ImageUrl,
                       Guid.Parse( userIdClaim ) // or int, whatever your ID type is
                   );
                   var result = await sender.Send( command );
                   if ( result.IsFailure )
                       return Results.BadRequest( result.Error );
                   return Results.Ok( new { collectionId = result.Value } );

                   // var userId = Guid.Parse( userIdClaim ); // or int, whatever your ID type is
                   // var movieItems = await repo.GetAllMovies();
                   // return Results.Ok( movieItems );
               } )
           .WithTags( Tags.Collections );
    }
}