using ExchangeRateUpdater.WebApi.Models;

namespace ExchangeRateUpdater.WebApi.Services.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
