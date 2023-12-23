using ExchangeRateUpdater.Model.ExchangeRates;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates;

public interface IExchangeRateDataSource
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken);
}