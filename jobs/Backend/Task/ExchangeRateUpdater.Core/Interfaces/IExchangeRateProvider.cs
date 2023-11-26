using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}