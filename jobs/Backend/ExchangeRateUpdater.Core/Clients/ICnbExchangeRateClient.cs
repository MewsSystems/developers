using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Clients
{
    /// <summary>
    /// Client interface for retrieving CNB exchange rate data.
    /// </summary>
    public interface ICnbExchangeRateClient
    {
        /// <summary>
        /// Gets the raw CnbRateResponse from the CNB API for the specified date.
        /// </summary>
        /// /// <param name="dateString">The date in **yyyy-MM-dd** format.</param>
        /// <returns>
        /// A *CnbRateResponse* object.
        /// </returns>
        Task<CnbRateResponse?> GetRatesAsync(string dateString);
    }
}
