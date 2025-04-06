using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Rankomizer.Domain.Catalog;

public class Song: Item
{
    public string Title { get; set; } = string.Empty;
    public string? Artist { get; set; }
    public string? Album { get; set; }

    public JsonDocument? SourceJson { get; set; }
}