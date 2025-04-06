using System.Text.Json;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Infrastructure.DTOs;

public class CollectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }

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
public class RosterItemDto
{
    public Guid Id { get; set; }
    public RosterItemStatus Status { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public double Score { get; set; }

    public ItemType ItemType { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public object? Details { get; set; } // Type-specific fields (MovieDetailsDto, etc.)
}

