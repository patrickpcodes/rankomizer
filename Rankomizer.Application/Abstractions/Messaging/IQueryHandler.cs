using MediatR;
using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
