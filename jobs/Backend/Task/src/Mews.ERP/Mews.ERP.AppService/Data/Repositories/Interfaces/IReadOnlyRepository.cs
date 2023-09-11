namespace Mews.ERP.AppService.Data.Repositories.Interfaces;

public interface IReadOnlyRepository<TEntity> where TEntity : class
{
    Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}