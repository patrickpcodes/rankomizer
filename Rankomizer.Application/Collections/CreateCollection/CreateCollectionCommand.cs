using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.Catalog;

namespace Rankomizer.Application.Collections.CreateCollection;

public sealed record CreateCollectionCommand(
    string Name,
    string Description,
    string ImageUrl,
    Guid UserId
) : ICommand<Collection>;

