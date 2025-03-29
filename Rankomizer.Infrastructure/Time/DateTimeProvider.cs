using Rankomizer.Domain.Abstractions;

namespace Rankomizer.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}