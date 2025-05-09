using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Rankomizer.Application.Collections.CreateCollection;
using Rankomizer.Application.Collections.GetCollectionById;

namespace Rankomizer.Server.Api.Endpoints.Collections;

public class GetCollectionById : IEndpoint
{
    public void MapEndpoint( IEndpointRouteBuilder app )
    {
        app.MapGet( "/api/collections/{collectionId}", [Authorize]
               async ( HttpContext context, ISender sender, Guid collectionId ) =>
               {
                   var command = new GetCollectionByIdQuery( collectionId );
                   var result = await sender.Send( command );
                   if ( result.IsFailure )
                       return Results.BadRequest( result.Error );
                   return Results.Ok( new { collection = result.Value } );
               } )
           .WithTags( Tags.Collections );
    }
}