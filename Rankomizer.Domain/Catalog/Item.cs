using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Rankomizer.Domain.Catalog;

public abstract class Item
{
    [Key]
    [JsonInclude]
    public Guid ItemId { get; set; }
   
    [JsonInclude] 
    public string Name { get; set; } = null!;
  
    [JsonInclude]
    public string Description { get; set; }
    
    [JsonInclude]
    public string ImageUrl { get; set; }

    public ItemType ItemType { get; set; }
    
    [JsonInclude]
    public DateTime CreatedAt { get; set; }
    [JsonInclude]
    public DateTime UpdatedAt { get; set; }
}

public enum ItemType
{
    Movie,
    Song,
    Painting
    // Add more later
}