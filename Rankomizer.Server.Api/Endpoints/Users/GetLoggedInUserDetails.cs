using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rankomizer.Application.Users.GetLoggedInUser;
using Rankomizer.Domain.User;
using Rankomizer.Server.Api.Extensions;
using Rankomizer.Server.Api.Infrastructure;

namespace Rankomizer.Server.Api.Endpoints.Users;

internal sealed class GetLoggedInUserDetails : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/details", [Authorize] async  (HttpContext context, ISender sender, CancellationToken cancellationToken) =>
           {
                var userIdClaim = context.User.FindFirst( ClaimTypes.NameIdentifier )?.Value;
              var userId = Guid.Parse( userIdClaim );
              
               var result = await sender.Send(new GetLoggedInUserDetailsQuery(userId));

               // Even if this fails, user is logged out on client side
               return result.Match(
                   _ => Results.Ok(result.Value),
                   CustomResults.Problem
               );
           })
           .WithTags(Tags.Users);
    }
}