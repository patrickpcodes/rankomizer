using Rankomizer.Application.Abstractions.Messaging;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Application.Collections.GetCollections;

public sealed record GetCollectionsQuery(Guid userId): IQuery<List<CollectionDto>>;