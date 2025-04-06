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
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Painting> Paintings => Set<Painting>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionItem> CollectionItems => Set<CollectionItem>();
    
    public DbSet<Gauntlet> Gauntlets => Set<Gauntlet>();
    public DbSet<RosterItem> RosterItems => Set<RosterItem>();
    public DbSet<Duel> Duels => Set<Duel>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // this is critical for Identity
        modelBuilder.Entity<Item>().ToTable("Items");
        
        modelBuilder.Entity<Movie>().ToTable("Movies");
        modelBuilder.Entity<Movie>()
                    .Property(m => m.SourceJson)
                    .HasColumnType("jsonb");
        modelBuilder.Entity<Song>().ToTable("Songs");
        modelBuilder.Entity<Song>()
                    .Property(m => m.SourceJson)
                    .HasColumnType("jsonb");
        modelBuilder.Entity<Painting>().ToTable("Paintings");

        modelBuilder.Entity<Collection>().ToTable("Collections");
        modelBuilder.Entity<CollectionItem>().ToTable("CollectionItems");
        
        modelBuilder.Entity<Gauntlet>().ToTable("Gauntlets");
        modelBuilder.Entity<RosterItem>().ToTable("RosterItems");
        modelBuilder.Entity<Duel>().ToTable("Duels");
        
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
        foreach (var entry in ChangeTracker.Entries<Item>())
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