using ExchangeRateProviderAPI_PaolaRojas.Models;
using ExchangeRateProviderAPI_PaolaRojas.Models.Responses;

namespace ExchangeRateProviderAPI_PaolaRojas.Services
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse> GetExchangeRatesAsync(IEnumerable<Currency> requestedCurrencies);
    }
}
