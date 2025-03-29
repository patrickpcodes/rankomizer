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
    public DbSet<CatalogEntry> Items => Set<CatalogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // this is critical for Identity
        modelBuilder.Entity<CatalogEntry>().ToTable("Items");

        modelBuilder.Entity<CatalogEntry>()
                    .Property(i => i.JsonData)
                    .HasColumnType("jsonb");

        modelBuilder.Entity<CatalogEntry>()
                    .Property(i => i.ItemName)
                    .HasMaxLength(255);

        modelBuilder.Entity<CatalogEntry>()
                    .Property(i => i.ItemType)
                    .HasMaxLength(50);
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