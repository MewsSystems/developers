using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Currency>> GetAllCurrenciesOrderedAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Currency>> GetActiveCurrenciesAsync(CancellationToken cancellationToken = default);
}
