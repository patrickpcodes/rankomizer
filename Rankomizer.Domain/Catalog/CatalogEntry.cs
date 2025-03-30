using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Rankomizer.Domain.Catalog;

public abstract class CatalogEntry
{
    [Key]
    [JsonInclude]
    public Guid ItemId { get; set; }
    [JsonInclude]
    public DateTime CreatedAt { get; set; }
    [JsonInclude]
    public DateTime UpdatedAt { get; set; }
}