using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure.Database;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

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
      public static async Task SeedItemsAsync(ApplicationDbContext context, IConfiguration configuration)
    {
        if ( !await context.Movies.AnyAsync() )
        { 
            
            var movieList = new List<MovieItem>();
          var tmdbApiKey = configuration["Tmdb:ApiKey"];
          TMDbClient client = new TMDbClient(tmdbApiKey);
          
          SearchContainer<SearchCollection> collectons = client.SearchCollectionAsync("James Bond").Result;
          Console.WriteLine($"Got {collectons.Results.Count:N0} collections");

          Collection jamesBonds = client.GetCollectionAsync(collectons.Results.First().Id).Result;
          Console.WriteLine($"Collection: {jamesBonds.Name}");
          Console.WriteLine();

          Console.WriteLine($"Got {jamesBonds.Parts.Count:N0} James Bond Movies");
          foreach ( SearchMovie part in jamesBonds.Parts )
          {
              var movie = client.GetMovieAsync(part.Id).Result;
              movieList.Add(new MovieItem()
              {
                  ItemId = new Guid(),
                  TmdbId = movie.Id,
                  ImdbId = movie.ImdbId,
                  SourceJson = JsonDocument.Parse(JsonSerializer.Serialize(movie, JsonOptions.CamelCase)) 
              }  ); 
          }
          context.Movies.AddRange(movieList);
          await context.SaveChangesAsync();
        }


        if( await context.Songs.AnyAsync() || await context.Paintings.AnyAsync()) return;

        var songs = new List<Song>
        {
            new() { ItemId = Guid.NewGuid(), Title = "Bohemian Rhapsody", Description = "Epic rock ballad", ImageUrl = "https://example.com/bohemian.jpg", Artist = "Queen", Album = "A Night at the Opera", ReleaseYear = 1975, Duration = "00:05:55" },
            new() { ItemId = Guid.NewGuid(), Title = "Imagine", Description = "John Lennon's iconic song", ImageUrl = "https://example.com/imagine.jpg", Artist = "John Lennon", Album = "Imagine", ReleaseYear = 1971, Duration = "00:03:01" },
            new() { ItemId = Guid.NewGuid(), Title = "Smells Like Teen Spirit", Description = "Grunge anthem", ImageUrl = "https://example.com/spirit.jpg", Artist = "Nirvana", Album = "Nevermind", ReleaseYear = 1991, Duration = "00:05:01" },
        };

        var paintings = new List<Painting>
        {
            new() { ItemId = Guid.NewGuid(), Title = "Starry Night", Description = "Post-Impressionist masterpiece", ImageUrl = "https://example.com/starrynight.jpg", Artist = "Vincent van Gogh", YearCreated = 1889, Medium = "Oil on canvas", Location = "MoMA" },
            new() { ItemId = Guid.NewGuid(), Title = "Mona Lisa", Description = "Leonardo da Vinci's iconic portrait", ImageUrl = "https://example.com/monalisa.jpg", Artist = "Leonardo da Vinci", YearCreated = 1503, Medium = "Oil on wood", Location = "Louvre" },
            new() { ItemId = Guid.NewGuid(), Title = "The Persistence of Memory", Description = "Surrealist melting clocks", ImageUrl = "https://example.com/memory.jpg", Artist = "Salvador Dalí", YearCreated = 1931, Medium = "Oil on canvas", Location = "MoMA" },
        };

        
        context.Songs.AddRange(songs);
        context.Paintings.AddRange(paintings);

        await context.SaveChangesAsync();
    }
}