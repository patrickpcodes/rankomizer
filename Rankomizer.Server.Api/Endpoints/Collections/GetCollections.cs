using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Collections.GetCollectionById;
using Rankomizer.Application.Collections.GetCollections;

namespace Rankomizer.Server.Api.Endpoints.Collections;

internal sealed class GetCollections : IEndpoint
{
    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapGet( "/api/collections", [Authorize]
               async ( HttpContext context, ISender sender ) => {
                   var userIdClaim = context.User.FindFirst( ClaimTypes.NameIdentifier )?.Value;
                   if ( userIdClaim == null )
                       return Results.Unauthorized();
                   
                   var userId = Guid.Parse( userIdClaim ); // or int, whatever your ID type is
                   var command = new GetCollectionsQuery( userId );
                   var result = await sender.Send( command );
                   if ( result.IsFailure )
                       return Results.BadRequest( result.Error );
                   return Results.Ok( new { collections = result.Value } );
               } )
           .WithTags( Tags.Collections );
    }
}