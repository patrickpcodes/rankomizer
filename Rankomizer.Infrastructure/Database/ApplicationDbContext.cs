using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Abstractions.Data;
using Rankomizer.Domain.User;

namespace Rankomizer.Infrastructure.Database;

public sealed class ApplicationDbContext( DbContextOptions<ApplicationDbContext> options, IPublisher publisher )
    :IdentityDbContext<ApplicationUser, ApplicationRole, Guid>( options ), IApplicationDbContext
{

}