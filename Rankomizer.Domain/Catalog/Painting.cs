using System.ComponentModel.DataAnnotations;

namespace Rankomizer.Domain.Catalog;

public class Painting: CatalogEntry
{

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Artist { get; set; }
    public int YearCreated { get; set; }
    public string? Medium { get; set; }
    public string? Location { get; set; }
}