using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Gauntlets;

public interface IGauntletService
{
    Task<Domain.Catalog.Gauntlet> CreateGauntletFromCollectionAsync( Guid collectionId, Guid userId,
                                                                     string? gauntletName = null );

    Task<Duel?> GetNextPendingDuelAsync(Guid gauntletId);
    Task<Duel?> SubmitDuelResultAsync(Guid duelId, Guid winnerRosterItemId);
    Task<List<RosterItem>> GetRosterAsync(Guid gauntletId);
}