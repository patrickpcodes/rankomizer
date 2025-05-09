using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rankomizer.Application.Tmdb;
using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Infrastructure.Tmdb;
using TMDbLib.Objects.Search;
using Movie = Rankomizer.Domain.Catalog.Movie;
namespace Rankomizer.Server.Api.Seeding;

public static class ItemSeeding
{
    
    public static async Task SeedMovieCollectionAsync( ApplicationDbContext context, ITmdbManager tmdbManager,
                                                       ApplicationUser user, int collectionId )
    {
        var tmdbCollection = await tmdbManager.GetTmdbCollectionAsync( collectionId );
        await FileSeeding.SaveJson<TMDbLib.Objects.Collections.Collection>( FileSeeding.TmdbCollection,
            $"{collectionId}", tmdbCollection );
        var collection = TmdbConverter.GetCollectionFromTmdbCollection( tmdbCollection );
        collection.SetUser(user);
        var movieList = new List<Movie>();
        foreach ( SearchMovie part in tmdbCollection.Parts )
        {
            var movie = await tmdbManager.GetTmdbMovieAsync( part.Id );
            await FileSeeding.SaveJson<TMDbLib.Objects.Movies.Movie>( FileSeeding.TmdbMovie,
                $"{movie.Id}", movie );
            movieList.Add( TmdbConverter.GetMovieFromTmdbMovie(movie) );
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
        Console.WriteLine( $"Seeded Collection {collection.Name} with {movieList.Count} movies." );
    }


    public static async Task SeedItemsAsync( ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                             ITmdbManager tmdbManager, IConfiguration configuration )
    {
        if ( !await context.Movies.AnyAsync() )
        {
            
                var collectionIds = new List<int>() { 645, 10, 230, 119, 1241, 9485, 131292, 87359 };

                var userRecord = configuration.GetSection( "system" ).Get<UserRecord>();
                var user = await userManager.FindByEmailAsync( userRecord.Email );
                if ( user == null )
                {
                    throw new Exception( "Poweruser not found" );
                }

                foreach ( var collectionId in collectionIds )
                {
                    await SeedMovieCollectionAsync( context, tmdbManager, user, collectionId );
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