using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider;

namespace ExchangeRateUpdater.ExchangeRate.Factory
{
    /// <summary>
    /// Factory interface for creating exchange rate providers.
    /// </summary>
    internal interface IExchangeRateProviderFactory
    {
        /// <summary>
        /// Gets the appropriate exchange rate provider for the specified currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>An instance of the exchange rate provider.</returns>
        IExchangeRateProvider GetProvider(Currency currency);
    }
}
