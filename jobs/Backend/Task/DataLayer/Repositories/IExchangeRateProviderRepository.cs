using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface IExchangeRateProviderRepository : IRepository<ExchangeRateProvider>
{
    Task<ExchangeRateProvider?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRateProvider>> GetActiveProvidersAsync(CancellationToken cancellationToken = default);
    Task<ExchangeRateProvider?> GetWithConfigurationsAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateHealthStatusAsync(int providerId, bool success, CancellationToken cancellationToken = default);
}
