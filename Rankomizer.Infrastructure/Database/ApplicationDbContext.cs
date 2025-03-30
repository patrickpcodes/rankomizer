using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Abstractions.Data;
using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.User;

namespace Rankomizer.Infrastructure.Database;

public sealed class ApplicationDbContext( DbContextOptions<ApplicationDbContext> options, IPublisher publisher )
    :IdentityDbContext<ApplicationUser, ApplicationRole, Guid>( options ), IApplicationDbContext
{
    public DbSet<MovieItem> Movies => Set<MovieItem>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Painting> Paintings => Set<Painting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // this is critical for Identity
        modelBuilder.Entity<MovieItem>().HasBaseType<CatalogEntry>();
        modelBuilder.Entity<MovieItem>()
                    .Property(m => m.SourceJson)
                    .HasColumnType("jsonb");
        modelBuilder.Entity<MovieItem>().ToTable("Movies");
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var utcNow = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<CatalogEntry>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                entry.Entity.UpdatedAt = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
            }
        }
    }
}