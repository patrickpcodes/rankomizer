using Rankomizer.Domain.User;

namespace Rankomizer.Domain.Catalog;

public class Gauntlet
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RosterItem> RosterItems { get; set; } = new List<RosterItem>();
    public ICollection<Duel> Duels { get; set; } = new List<Duel>();
}
