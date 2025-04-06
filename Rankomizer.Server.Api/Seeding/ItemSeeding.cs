using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Database;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Collection = Rankomizer.Domain.Catalog.Collection;
using Movie = Rankomizer.Domain.Catalog.Movie;

namespace Rankomizer.Server.Api.Seeding;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public static class CustomParse<T> where T : class
{
    public static T Parse( string jsonString )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Deserialize<T>( jsonString, options );
    }
}

public static class ItemSeeding
{
    public static async Task SeedMovieCollectionAsync( ApplicationDbContext context, TMDbClient client,
                                                       ApplicationUser user, int collectionId )
    {
        var tmdbCollection = await client.GetCollectionAsync( collectionId );

        var collection = new Collection()
        {
            Name = "TMDB Collection: " + tmdbCollection.Name,
            Description = tmdbCollection.Overview,
            CreatedByUserId = user.Id,
            CreatedByUser = user,
            Items = new List<CollectionItem>()
        };
        var movieList = new List<Movie>();
        foreach ( SearchMovie part in tmdbCollection.Parts )
        {
            var movie = client.GetMovieAsync( part.Id ).Result;
            movieList.Add( new Movie()
            {
                ItemId = Guid.NewGuid(),
                Name = movie.Title,
                Description = movie.Overview,
                ImageUrl = movie.PosterPath != null
                    ? $"https://image.tmdb.org/t/p/w500{movie.PosterPath}"
                    : null,
                ItemType = ItemType.Movie,
                TmdbId = movie.Id,
                ImdbId = movie.ImdbId,
                ReleaseDate = movie.ReleaseDate == null ? new DateTime() : DateTime.SpecifyKind(movie.ReleaseDate.Value, DateTimeKind.Utc),
                SourceJson = JsonDocument.Parse( JsonSerializer.Serialize( movie, JsonOptions.CamelCase ) )
            } );
        }
        context.Movies.AddRange( movieList );
        var order = 0;
        foreach ( var movie in movieList )
        {
            collection.Items.Add( new CollectionItem
            {
                Item = movie,
                Order = order++,
            } );
        }
        context.Collections.Add( collection );
        await context.SaveChangesAsync();
        Console.WriteLine($"Seeded Collection {collection.Name} with {movieList.Count} movies.");
        // var movie_collection = JsonSerializer.Serialize( collection, JsonOptions.CamelCase );
        // // // Save the JSON string to a file
        // var filePath = Path.Combine( Directory.GetCurrentDirectory(), "SeedData", collection.Name+"_collection.json" );
        // await File.WriteAllTextAsync( filePath, movie_collection );
        //
        // var movie_list = JsonSerializer.Serialize( collection, JsonOptions.CamelCase );
        // // // Save the JSON string to a file
        // filePath = Path.Combine( Directory.GetCurrentDirectory(), "SeedData", collection.Name+"_movie_list.json" );
        // await File.WriteAllTextAsync( filePath, movie_list );
    }


    public static async Task SeedItemsAsync( ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                             IConfiguration configuration )
    {
        if ( !await context.Movies.AnyAsync() )
        {
            var movieListFileName = "initial_movie_list.json";
            //Read in JArray from file
            var filpath = Path.Combine( Directory.GetCurrentDirectory(), "SeedData", movieListFileName );
            if ( File.Exists( filpath ) )
            {
                var t = await File.ReadAllTextAsync( filpath );
                var movieItemsFromFile = CustomParse<List<Movie>>.Parse( t );

                if ( movieItemsFromFile != null )
                {
                    context.Movies.AddRange( movieItemsFromFile );
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                var movieList = new List<Movie>();
                var searchMovieList = new List<SearchMovie>();
                var tmdbApiKey = configuration["Tmdb:ApiKey"];
                TMDbClient client = new TMDbClient( tmdbApiKey );
                var collectionIds = new List<int>() { 645, 10, 230, 119 };
                var userRecord = configuration.GetSection( "poweruser" ).Get<UserRecord>();
                var user = await userManager.FindByEmailAsync( userRecord.Email );
                if ( user == null )
                {
                    throw new Exception( "Poweruser not found" );
                }

                foreach ( var collectionId in collectionIds )
                {
                    await SeedMovieCollectionAsync( context, client, user, collectionId );
                    // var collection = await client.GetCollectionAsync( collectionId );
                    // searchMovieList.AddRange( collection.Parts );
                }


                // context.Movies.AddRange( movieList );
                // await context.SaveChangesAsync();
                //
                // var movies = await context.Movies.ToListAsync();
                // var jsonString = JsonSerializer.Serialize( movies, JsonOptions.CamelCase );
                // // Save the JSON string to a file
                // var filePath = Path.Combine( Directory.GetCurrentDirectory(), "SeedData", movieListFileName );
                // await File.WriteAllTextAsync( filePath, jsonString );
            }
        }

        return;
        if ( await context.Songs.AnyAsync() || await context.Paintings.AnyAsync() ) return;

        var songs = new List<Song>
        {
            new()
            {
                ItemId = Guid.NewGuid(), Title = "Bohemian Rhapsody", Description = "Epic rock ballad",
                ImageUrl = "https://example.com/bohemian.jpg", Artist = "Queen", Album = "A Night at the Opera",
            },
            new()
            {
                ItemId = Guid.NewGuid(), Title = "Imagine", Description = "John Lennon's iconic song",
                ImageUrl = "https://example.com/imagine.jpg", Artist = "John Lennon", Album = "Imagine",
            },
            new()
            {
                ItemId = Guid.NewGuid(), Title = "Smells Like Teen Spirit", Description = "Grunge anthem",
                ImageUrl = "https://example.com/spirit.jpg", Artist = "Nirvana", Album = "Nevermind",
            },
        };

        var paintings = new List<Painting>
        {
            new()
            {
                ItemId = Guid.NewGuid(), Title = "Starry Night", Description = "Post-Impressionist masterpiece",
                ImageUrl = "https://example.com/starrynight.jpg", Artist = "Vincent van Gogh", YearCreated = 1889,
                Medium = "Oil on canvas", Location = "MoMA"
            },
            new()
            {
                ItemId = Guid.NewGuid(), Title = "Mona Lisa", Description = "Leonardo da Vinci's iconic portrait",
                ImageUrl = "https://example.com/monalisa.jpg", Artist = "Leonardo da Vinci", YearCreated = 1503,
                Medium = "Oil on wood", Location = "Louvre"
            },
            new()
            {
                ItemId = Guid.NewGuid(), Title = "The Persistence of Memory", Description = "Surrealist melting clocks",
                ImageUrl = "https://example.com/memory.jpg", Artist = "Salvador Dalí", YearCreated = 1931,
                Medium = "Oil on canvas", Location = "MoMA"
            },
        };


        context.Songs.AddRange( songs );
        context.Paintings.AddRange( paintings );

        await context.SaveChangesAsync();
    }
}