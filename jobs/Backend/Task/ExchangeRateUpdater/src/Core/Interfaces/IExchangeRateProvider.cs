using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces
{
    /// <summary>
    /// Provides functionality to retrieve exchange rates for specified currencies.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets exchange rates for the specified currencies.
        /// </summary>
        /// <param name="currencies">Collection of currencies to get exchange rates for.</param>
        /// <returns>Collection of exchange rates for the specified currencies.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
