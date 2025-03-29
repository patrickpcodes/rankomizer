using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;

namespace Rankomizer.Server.Api.Seeding;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public static class ItemSeeding
{
    public static async Task SeedItemsAsync(ApplicationDbContext context)
    {
        if (await context.Items.AnyAsync()) return;

        var movie = new MovieMetadata
        {
            Director = "Christopher Nolan",
            ReleaseYear = 2010,
            Genre = "Sci-Fi"
        };

        var song = new SongMetadata
        {
            Artist = "Queen",
            Album = "A Night at the Opera",
            ReleaseYear = 1975,
            Duration = "00:05:55"
        };

        var painting = new PaintingMetadata
        {
            Artist = "Vincent van Gogh",
            YearCreated = 1889,
            Medium = "Oil on canvas",
            Location = "Museum of Modern Art"
        };

        var items = new List<CatalogEntry>
        {
            new CatalogEntry
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Inception",
                ItemType = "Movie",
                Description = "A mind-bending thriller",
                ImageUrl = "https://example.com/inception.jpg",
                JsonData = JsonDocument.Parse(JsonSerializer.Serialize(movie, JsonOptions.CamelCase))
            },
            new CatalogEntry
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Bohemian Rhapsody",
                ItemType = "Song",
                Description = "Epic rock ballad",
                ImageUrl = "https://example.com/bohemian.jpg",
                JsonData = JsonDocument.Parse(JsonSerializer.Serialize(song, JsonOptions.CamelCase))
            },
            new CatalogEntry
            {
                ItemId = Guid.NewGuid(),
                ItemName = "Starry Night",
                ItemType = "Painting",
                Description = "Post-Impressionist masterpiece",
                ImageUrl = "https://example.com/starrynight.jpg",
                JsonData = JsonDocument.Parse(JsonSerializer.Serialize(painting, JsonOptions.CamelCase))
            }
        };

        context.Items.AddRange(items);
        await context.SaveChangesAsync();
    }
}