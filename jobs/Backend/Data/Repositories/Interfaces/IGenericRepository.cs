using System.Linq.Expressions;

namespace Data.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    T? LastElement();
}