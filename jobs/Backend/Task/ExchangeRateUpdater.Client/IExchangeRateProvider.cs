using ExchangeRateUpdater.Client.Contracts;

namespace ExchangeRateUpdater.Client;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}