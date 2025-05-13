using ExchangeRateUpdater.Entities;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}
