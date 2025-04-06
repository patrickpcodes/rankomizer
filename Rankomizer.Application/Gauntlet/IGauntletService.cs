using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Gauntlet;

public interface IGauntletService
{
    Task<Domain.Catalog.Gauntlet> CreateGauntletFromCollectionAsync( Guid collectionId, Guid userId,
                                                                     string? gauntletName = null );

    Task<Duel?> GetNextPendingDuelAsync(Guid gauntletId);
    Task<Duel?> SubmitDuelResultAsync(Guid duelId, Guid winnerRosterItemId);
}