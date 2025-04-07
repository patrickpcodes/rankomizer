using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rankomizer.Application.Users;
using Rankomizer.Application.Users.LogoutUser;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;
using Rankomizer.Server.Api.Extensions;
using Rankomizer.Server.Api.Infrastructure;

namespace Rankomizer.Server.Api.Endpoints.Users;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/logout", [Authorize] async  (HttpContext context,UserManager<ApplicationUser> userManager, ISender sender, CancellationToken cancellationToken) =>
           {
               // Always delete the cookie first
               context.Response.Cookies.Delete("rankomizer-jwt", new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,
                   SameSite = SameSiteMode.None,
                   Expires = DateTimeOffset.Now.AddMinutes(300)
               });

               // Try to load user and log logout event
               var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               if (string.IsNullOrWhiteSpace(userId))
                   return Results.Ok(new { message = "Logged out" }); // still OK, just no logging

               var user = await userManager.FindByIdAsync(userId);

               if (user is null)
                   return Results.Ok(new { message = "Logged out" }); // same here

               var result = await sender.Send(new LogoutUserCommand(user));

               // Even if this fails, user is logged out on client side
               return result.Match(
                   _ => Results.Ok(new { message = "Logged out" }),
                   CustomResults.Problem
               );
           })
           .WithTags(Tags.Users);
    }
}