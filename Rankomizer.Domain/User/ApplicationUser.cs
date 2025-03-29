using Microsoft.AspNetCore.Identity;

namespace Rankomizer.Domain.User;

public class ApplicationUser : IdentityUser<Guid>
{

}

public class ApplicationRole : IdentityRole<Guid>
{
    
}