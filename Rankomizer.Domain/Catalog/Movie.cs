using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Rankomizer.Domain.Catalog;

public class Movie : Item
{
    public int TmdbId { get; set; }
    public string ImdbId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public JsonDocument? SourceJson { get; set; }
}