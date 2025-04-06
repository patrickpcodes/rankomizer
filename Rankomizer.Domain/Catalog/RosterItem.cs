namespace Rankomizer.Domain.Catalog;

public class RosterItem
{
    public Guid Id { get; set; }

    public Guid GauntletId { get; set; }
    public Gauntlet Gauntlet { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }

    public RosterItemStatus Status { get; set; } = RosterItemStatus.Active;

    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;
    public double Score { get; set; } = 0; // we can define how to calculate it later
}

public enum RosterItemStatus
{
    Active,
    Unseen,
    Skipped
}
