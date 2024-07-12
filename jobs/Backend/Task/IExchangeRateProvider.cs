using ExchangeRateUpdater.Application;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}