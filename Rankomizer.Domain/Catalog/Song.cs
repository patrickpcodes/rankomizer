using System.ComponentModel.DataAnnotations;

namespace Rankomizer.Domain.Catalog;

public class Song: CatalogEntry
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public int ReleaseYear { get; set; }
    public string? Duration { get; set; }
}