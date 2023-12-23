using ExchangeRateUpdater.Model.ExchangeRates;

namespace ExchangeRateUpdater.Services.ExchangeRates;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}