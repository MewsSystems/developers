using System.Threading.Tasks;

namespace ExchangeRateUpdater.HttpClients
{
    // Interface for HTTP client that communicates with CNB API.
    public interface ICnbHttpClient
    {
        // Method's signature: fetches exchange rate data from the CNB API and returns exchange rate data as a string in pipe-separated format
        Task<string> GetExchangeRatesAsync();
    }
}
