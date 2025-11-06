using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface IExchangeRateRepository : IRepository<ExchangeRate>
{
    Task<IEnumerable<ExchangeRate>> GetCurrentRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRate>> GetRatesByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRate>> GetRatesByCurrencyPairAsync(int baseCurrencyId, int targetCurrencyId, CancellationToken cancellationToken = default);
    Task<ExchangeRate?> GetRateAsync(int providerId, int baseCurrencyId, int targetCurrencyId, DateOnly validDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExchangeRate>> GetRatesByProviderAndDateRangeAsync(int providerId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}
