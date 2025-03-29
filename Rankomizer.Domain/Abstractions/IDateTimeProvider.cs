namespace Rankomizer.Domain.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}