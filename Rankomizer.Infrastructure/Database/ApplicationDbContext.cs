using MediatR;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Abstractions.Data;

namespace Rankomizer.Infrastructure.Database;

public sealed class ApplicationDbContext( DbContextOptions<ApplicationDbContext> options, IPublisher publisher )
    : DbContext( options ), IApplicationDbContext
{

}