using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? day, CancellationToken cancellationToken = default);
}
