using ExchangeRateUpdater.Entities;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateRepository
{
    Task<IEnumerable<ExchangeRate>> GetCzkExchangeRatesAsync(CancellationToken cancellationToken = default);
}
