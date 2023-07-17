using ExchangeRateUpdater;
using ExchangeRateWebApi.Models;

namespace ExchangeRateWebApi.Services
{
    public interface IExchangeRate
    {
        List<DataNode> GetData(string url);
        IEnumerable<ExchangeRate> MapDataToExchangeRates();
    }
}
