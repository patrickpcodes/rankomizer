namespace Rankomizer.Application.Abstractions.Data;

public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}