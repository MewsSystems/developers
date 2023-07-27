using ExchangeRatesProvider.Models;

namespace ExchangeRatesProvider.Interfaces
{
    public interface IExchangeRateService
    {
        Task<(RatesViewModel result, int statusCode)> GetExchangeRates();
        Task<(RatesViewModel result, int statusCode)> GetSearchResults(string search);
        Task<(RatesViewModel result, int statusCode)> GetSelectedCurrencies();
    }
}
