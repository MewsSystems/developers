using ExchangeRateUpdater;
using ExchangeRateWebApi.Models;

namespace ExchangeRateWebApi.Services
{
    public interface IExchangeRate
    {
        Task<List<DataNode>> GetDataAsync(string url);
        Task<IEnumerable<ExchangeRate>> MapDataToExchangeRatesAsync();
    }
}
