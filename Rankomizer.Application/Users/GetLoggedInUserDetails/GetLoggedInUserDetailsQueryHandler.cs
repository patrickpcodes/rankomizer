using Microsoft.AspNetCore.Identity;
using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;

namespace Rankomizer.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserDetailsQueryHandler
    : IQueryHandler<GetLoggedInUserDetailsQuery, UserDetailsResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public GetLoggedInUserDetailsQueryHandler( UserManager<ApplicationUser> userManager )
    {
        _userManager = userManager;
    }
    
    public async Task<Result<UserDetailsResponse>> Handle( GetLoggedInUserDetailsQuery request, CancellationToken cancellationToken )
    {
        //TODO Better?  Pass in claims principal
        var user = await _userManager.FindByIdAsync( request.userId.ToString() );
        if ( user == null )
        {
            return ApplicationUserErrors.NotFound( request.userId );
        }

        var response = new UserDetailsResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        };

        return Result<UserDetailsResponse>.Success( response );
    }
}