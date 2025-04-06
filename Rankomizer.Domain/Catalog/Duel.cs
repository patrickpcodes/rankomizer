namespace Rankomizer.Domain.Catalog;

public class Duel
{
    public Guid Id { get; set; }

    public Guid GauntletId { get; set; }
    public Gauntlet Gauntlet { get; set; }

    public Guid RosterItemAId { get; set; }
    public RosterItem RosterItemA { get; set; }

    public Guid RosterItemBId { get; set; }
    public RosterItem RosterItemB { get; set; }

    public Guid? WinnerRosterItemId { get; set; } // null = not yet answered
    public RosterItem? WinnerRosterItem { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
