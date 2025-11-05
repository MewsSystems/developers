using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface IExchangeRateProviderConfigurationRepository : IRepository<ExchangeRateProviderConfiguration>
{
    Task<IEnumerable<ExchangeRateProviderConfiguration>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default);
    Task<ExchangeRateProviderConfiguration?> GetByProviderAndKeyAsync(int providerId, string settingKey, CancellationToken cancellationToken = default);
}
