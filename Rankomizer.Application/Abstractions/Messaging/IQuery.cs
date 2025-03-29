using MediatR;
using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
