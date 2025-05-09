using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Application.Collections.GetCollectionById;

public sealed record GetCollectionByIdQuery(Guid collectionId): IQuery<CollectionDto>;