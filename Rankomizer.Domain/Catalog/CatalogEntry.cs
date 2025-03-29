using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json;

namespace Rankomizer.Domain.Catalog;

public class CatalogEntry
{
    [Key]
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string ItemType { get; set; } = string.Empty; // e.g., "Movie", "Song", etc.
    public JsonDocument JsonData { get; set; } = JsonDocument.Parse( "{}" );
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public T? GetDataAs<T>() => JsonData.Deserialize<T>();
}

public class MovieMetadata
{
    [JsonProperty( "director" )]
    public string? Director { get; set; }
    [JsonProperty( "releaseYear" )]
    public int ReleaseYear { get; set; }
    [JsonProperty( "genre" )]
    public string? Genre { get; set; }
}

public class SongMetadata
{
    [JsonProperty( "artist" )]
    public string? Artist { get; set; }
    [JsonProperty( "album" )]
    public string? Album { get; set; }
    [JsonProperty( "releaseYear" )]
    public int ReleaseYear { get; set; }
    [JsonProperty( "duration" )]
    public string? Duration { get; set; }
}

public class PaintingMetadata
{
    [JsonProperty( "artist" )]
    public string? Artist { get; set; }
    [JsonProperty( "yearCreated" )]
    public int YearCreated { get; set; }
    [JsonProperty( "medium" )]
    public string? Medium { get; set; }
    [JsonProperty( "location" )]
    public string? Location { get; set; }
}