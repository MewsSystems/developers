using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract
{
    public interface ICnbRatesFetcher
    {
        /// <summary>
        /// Fetches rates from CNB
        /// </summary>
        /// <param name="url">URL of source txt data</param>
        /// <returns>Fetched data contents</returns>
        Task<string> FetchRatesAsync(string url);
    }
}
