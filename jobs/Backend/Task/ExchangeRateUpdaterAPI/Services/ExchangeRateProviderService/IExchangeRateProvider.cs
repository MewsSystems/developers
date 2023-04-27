using ExchangeRateUpdater;

namespace ExchangeRateUpdaterAPI.Services.ExchangeRateProviderService
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}

