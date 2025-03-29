using MediatR;
using Rankomizer.Application.Users;
using Rankomizer.Application.Users.Login;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Server.Api.Extensions;
using Rankomizer.Server.Api.Infrastructure;

namespace Rankomizer.Server.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (HttpContext context, Request request, ISender sender, CancellationToken cancellationToken) =>
           {
               var command = new LoginUserCommand(request.Email, request.Password);

               Result<string> result = await sender.Send(command, cancellationToken);

               // return result.Match(Results.Ok, CustomResults.Problem);
               return result.Match(
                   token =>
                   {
                       context.Response.Cookies.Append("rankomizer-jwt", token, new CookieOptions
                       {
                           HttpOnly = true,
                           Secure = true, // set to true only if you're running on HTTPS
                           SameSite = SameSiteMode.None, // or Lax, depending on your frontend/backend setup
                           Expires = DateTimeOffset.Now.AddMinutes(300)
                       });

                       return Results.Ok(new { message = "Logged in" }); // you can return anything here
                   },
                   CustomResults.Problem
               );
           })
           .WithTags(Tags.Users);
    }
}