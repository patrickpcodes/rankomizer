namespace Rankomizer.Domain.Catalog;

public class CollectionItem
{
    public Guid Id { get; set; }

    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }

    public int Order { get; set; }  // Optional sort order
}
