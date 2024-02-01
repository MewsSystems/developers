using Mews.ERP.AppService.Data.Repositories.Interfaces;

namespace Mews.ERP.AppService.Data.Repositories;

public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    public abstract Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}