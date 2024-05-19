using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencies);
    }

}
