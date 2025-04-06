using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Gauntlet;

public interface IGauntletService
{
    Task<Domain.Catalog.Gauntlet> CreateGauntletFromCollectionAsync( Guid collectionId, Guid userId,
                                                                     string? gauntletName = null );
}