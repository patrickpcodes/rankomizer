using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Rankomizer.Domain.Catalog;

public class MovieItem : CatalogEntry
{

    public int TmdbId { get; set; }
    public string ImdbId { get; set; } 
    
    public JsonDocument? SourceJson { get; set; }
}