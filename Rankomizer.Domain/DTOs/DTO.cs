﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Domain.DTOs;

public static class DuelExtensions
{
    public static DuelDto ToDto( this Duel duel )
    {
        var gauntlet = duel.Gauntlet;

        return new DuelDto
        {
            DuelId = duel.Id,
            OptionA = duel.RosterItemA.ToDto(),
            OptionB = duel.RosterItemB.ToDto(),

        };
    }

    public static RosterItemDto ToDto( this RosterItem ri )
    {
        var item = ri.Item;

        return new RosterItemDto
        {
            Id = ri.Id,
            Status = ri.Status,
            Wins = ri.Wins,
            Losses = ri.Losses,
            Score = ri.Score,
            ItemType = item.ItemType,
            Name = item.Name,
            Description = item.Description,
            ImageUrl = item.ImageUrl,
            Details = item switch
            {
                Movie m => new MovieDetailsDto
                {
                    TmdbId = m.TmdbId,
                    ImdbId = m.ImdbId,
                    ReleaseDate = m.ReleaseDate,
                    // SourceJson = m.SourceJson,
                },
                // Song s => new SongDetailsDto
                // {
                //     Artist = s.Artist,
                //     Album = s.Album,
                //     DurationSeconds = s.DurationSeconds
                // },
                _ => null
            }
        };
    }

    public static CollectionDto ToDto( this Collection collection )
    {
        return new CollectionDto()
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            ImageUrl = collection.ImageUrl,
            Items = collection.Items.Select( i => new ItemDto
            {
                Id = i.ItemId,
                Name = i.Item.Name,
                Description = i.Item.Description,
                ImageUrl = i.Item.ImageUrl,
                ItemType = i.Item.ItemType,
                Details = i.Item switch
                {
                    Movie m => new MovieDetailsDto
                    {
                        TmdbId = m.TmdbId,
                        ImdbId = m.ImdbId,
                        ReleaseDate = m.ReleaseDate,
                        // SourceJson = m.SourceJson
                    },
                    // Song s => new SongDetailsDto
                    // {
                    //     Artist = s.Artist,
                    //     Album = s.Album,
                    //     DurationSeconds = s.DurationSeconds
                    // },
                    _ => null
                }
            } ).ToList()
        };
    }

    public static CompletedDuelDto ToCompletedDuelDto( this Duel duel )
    {
        return new CompletedDuelDto
        {
            DuelId = duel.Id,
            WinnerId = duel.WinnerRosterItemId.Value,
            Item1 = new MiniItemDto
            {
                Id = duel.RosterItemA.Id,
                Name = duel.RosterItemA.Item.Name,
                ImageUrl = duel.RosterItemA.Item.ImageUrl,
            },
            Item2 = new MiniItemDto
            {
                Id = duel.RosterItemB.Id,
                Name = duel.RosterItemB.Item.Name,
                ImageUrl = duel.RosterItemB.Item.ImageUrl,
            },
            UpdatedDate = duel.UpdatedAt,
        };
    }

}

public class CollectionDto
{
    [JsonPropertyName( "id" )]
    public Guid Id { get; set; }

    [JsonPropertyName( "name" )]
    public string Name { get; set; } = null!;

    [JsonPropertyName( "description" )]
    public string Description { get; set; }

    [JsonPropertyName( "imageUrl" )]
    public string ImageUrl { get; set; }

    public List<ItemDto> Items { get; set; } = new();
}



public class ItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public ItemType ItemType { get; set; }
    public object? Details { get; set; } // type-specific details
}

// type-specific payloads
public class MovieDetailsDto
{
    public int TmdbId { get; set; }
    public string ImdbId { get; set; }
    public DateTime ReleaseDate { get; set; }

    public JsonDocument? SourceJson { get; set; }
}

public class SongDetailsDto
{
    public string Artist { get; set; } = null!;
    public string Album { get; set; } = null!;
    public int DurationSeconds { get; set; }
}

public class GauntletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public List<RosterItemDto> Roster { get; set; } = new();
}

public class RosterItemStatusJsonConverter : JsonConverter<RosterItemStatus>
{
    public override RosterItemStatus Read( ref Utf8JsonReader reader, Type typeToConvert,
                                           JsonSerializerOptions options )
    {
        var value = reader.GetString();

        return value switch
        {
            "Active" => RosterItemStatus.Active,
            "Skipped" => RosterItemStatus.Skipped,
            //"Removed" => RosterItemStatus.Removed,
            _ => throw new JsonException( $"Unknown RosterItemStatus: {value}" )
        };
    }

    public override void Write( Utf8JsonWriter writer, RosterItemStatus value, JsonSerializerOptions options )
    {
        writer.WriteStringValue( value.ToString() ); // Outputs "Active", "Skipped", etc.
    }
}

public class ItemTypeJsonConverter : JsonConverter<ItemType>
{
    public override ItemType Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        var value = reader.GetString();

        return value switch
        {
            "Movie" => ItemType.Movie,
            "Song" => ItemType.Song,
            //"Removed" => RosterItemStatus.Removed,
            _ => throw new JsonException( $"Unknown RosterItemStatus: {value}" )
        };
    }

    public override void Write( Utf8JsonWriter writer, ItemType value, JsonSerializerOptions options )
    {
        writer.WriteStringValue( value.ToString() ); // Outputs "Active", "Skipped", etc.
    }
}

public class RosterItemDto
{
    [JsonPropertyName( "id" )]
    public Guid Id { get; set; }

    [JsonPropertyName( "status" )]
    [JsonConverter( typeof(RosterItemStatusJsonConverter) )]
    public RosterItemStatus Status { get; set; }

    [JsonPropertyName( "wins" )]
    public int Wins { get; set; }

    [JsonPropertyName( "losses" )]
    public int Losses { get; set; }

    [JsonPropertyName( "score" )]
    public double Score { get; set; }

    [JsonPropertyName( "itemType" )]
    [JsonConverter( typeof(ItemTypeJsonConverter) )]
    public ItemType ItemType { get; set; }

    [JsonPropertyName( "name" )]
    public string Name { get; set; } = null!;

    [JsonPropertyName( "description" )]
    public string Description { get; set; }

    [JsonPropertyName( "imageUrl" )]
    public string ImageUrl { get; set; }

    [JsonPropertyName( "details" )]

    public object? Details { get; set; } // Type-specific fields (MovieDetailsDto, etc.)

    public T? GetTypedDetails<T>()
    {
        if ( Details is JsonElement jsonElement )
        {
            // Make sure it's actually an object
            if ( jsonElement.ValueKind == JsonValueKind.Object )
            {
                return JsonSerializer.Deserialize<T>( jsonElement.GetRawText() );
            }
        }

        if ( Details is T typed )
        {
            return typed;
        }

        return default;
    }
}

public class MovieDetails
{
    [JsonPropertyName( "tmdbId" )]
    public int TmdbId { get; set; }

    [JsonPropertyName( "imdbId" )]
    public string ImdbId { get; set; }

    [JsonPropertyName( "releaseDate" )]
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName( "sourceJson" )]
    public string? SourceJson { get; set; }
}

public class DuelDto
{
    [JsonPropertyName( "duelId" )]
    public Guid DuelId { get; set; }

    [JsonPropertyName( "optionA" )]
    public RosterItemDto OptionA { get; set; }

    [JsonPropertyName( "optionB" )]
    public RosterItemDto OptionB { get; set; }
}

public class DuelResponseDto
{
    [JsonPropertyName( "duel" )]
    public DuelDto? Duel { get; set; }

    [JsonPropertyName( "roster" )]
    public List<RosterItemDto> Roster { get; set; } = new();

    [JsonPropertyName( "completedDuels" )]
    public List<CompletedDuelDto> CompletedDuels { get; set; } = new();
}

public class CompletedDuelDto
{
    [JsonPropertyName( "duelId" )]
    public Guid DuelId { get; set; }

    [JsonPropertyName( "item1" )]
    public MiniItemDto Item1 { get; set; }

    [JsonPropertyName( "item2" )]
    public MiniItemDto Item2 { get; set; }

    [JsonPropertyName( "winnerId" )]
    public Guid WinnerId { get; set; }
    [JsonPropertyName( "updatedDate" )]
    public DateTime UpdatedDate { get; set; }
}



public class MiniItemDto
{
    [JsonPropertyName( "id" )]
    public Guid Id { get; set; }

    [JsonPropertyName( "name" )]
    public string Name { get; set; }

    [JsonPropertyName( "imageUrl" )]
    public string ImageUrl { get; set; }
}

public class SubmitDuelRequest
{
    public Guid DuelId { get; set; }
    public Guid WinnerRosterItemId { get; set; }
}
