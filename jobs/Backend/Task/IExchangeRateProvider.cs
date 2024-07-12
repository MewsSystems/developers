using ExchangeRateUpdater.Application;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}