using Rankomizer.Domain.User;

namespace Rankomizer.Domain.Catalog;

public class Collection
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; }

    public Guid CreatedByUserId { get; set; }
    public ApplicationUser CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CollectionItem> Items { get; set; } = new List<CollectionItem>();
}
