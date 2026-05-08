using System.Threading.Tasks;
using System.Threading;

namespace ExchangeRateUpdater.Clients
{
    /// <summary>
    /// Generic contract for fetching exchange rate data from any source.
    /// Implementation can target different central banks or financial data providers.
    /// </summary>
    public interface IExchangeRateApiClient
    {
        /// <summary>
        /// Fetches raw exchange rate data from the source.
        /// </summary>
        Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default);
    }
}