using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Providers;

public interface IExchangeRateProvider
{
    Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}