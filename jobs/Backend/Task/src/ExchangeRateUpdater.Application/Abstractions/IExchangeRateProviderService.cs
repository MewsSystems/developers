using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Abstractions
{
    public interface IExchangeRateProviderService
    {
        Task<ExchangeRateModel> GetExchangeRates(IEnumerable<CurrencyModel> request);
    }
}
