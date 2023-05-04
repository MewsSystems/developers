using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// CNB API client providing daily and monthly currency exchange rates.
    /// </summary>
    public interface ICnbApiClient
    {
        /// <summary>
        /// Returns daily exchange rates listing provided by the CNB website.
        /// </summary>
        /// <returns>Daily exchange rates listing.</returns>
        public Task<ExchangeRatesListing> GetDailyExchangeRates();

        /// <summary>
        /// Returns monthly exchange rates listing (for other currencies not present in the daily listing) provided by the CNB website.
        /// </summary>
        /// <returns>Monthly exchange rates listing.</returns>
        public Task<ExchangeRatesListing> GetMonthlyExchangeRates();
    }
}
