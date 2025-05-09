using System.Text.Json;
using Rankomizer.Infrastructure.Json;

namespace Rankomizer.Server.Api.Seeding;

public static class FileSeeding
{
    public const string TmdbCollection = "tmdb-collection";
    public const string TmdbMovie = "tmdb-movie";
    
    public static async Task<T> ReadJson<T>( string foldername, string filename )
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SeedData",
            foldername,
            $"{filename}.json"
        );
        if ( !File.Exists( filePath ) )
        {
            throw new FileNotFoundException( $"File {filePath} not found." );
        }
        var json = await File.ReadAllTextAsync( filePath );
        return JsonSerializer.Deserialize<T>( json, JsonOptions.CamelCase );
    }
    public static async Task SaveJson<T>( string foldername, string filename, T obj )
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SeedData",
            foldername,
            $"{filename}.json"
        );
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var json = JsonSerializer.Serialize( obj, JsonOptions.CamelCase ); 
        await File.WriteAllTextAsync( filePath, json ); 
    }
    
}