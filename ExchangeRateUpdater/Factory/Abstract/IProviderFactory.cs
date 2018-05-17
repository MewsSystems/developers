using ExchangeRateUpdater.ExchangeRateStrategies;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Factory.Abstract
{
    public interface IProviderFactory
    {
        /// <summary>
        /// Returns exchange rate provider for specified source currency
        /// </summary>
        /// <param name="sourceCurrency">Source currency</param>
        /// <returns>Exchange rate provider</returns>
        IExchangeRateProviderSourceCurrencyStrategy GetSourceCurrencyProvider(Currency sourceCurrency);

        /// <summary>
        /// Returns exchange rate provider for specified target currency
        /// </summary>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Exchange rate provider</returns>
        IExchangeRateProviderTargetCurrencyStrategy GetTargetCurrencyProvider(Currency targetCurrency);

        /// <summary>
        /// Returns pairs of exchange rate providers and their base currencies for specified list of currencies
        /// </summary>
        /// <param name="currencies">List of currencies</param>
        /// <returns>List of exchange rate providers and their base currencies</returns>
        IEnumerable<ProviderInfo> GetProviders(IEnumerable<Currency> currencies);
    }
}
