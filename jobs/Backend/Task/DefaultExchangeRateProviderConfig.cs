using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Represents the default exchange rate provider configuration.
    /// </summary>
    public class DefaultExchangeRateProviderConfig : Dictionary<string, string>
    {
        /// <summary>
        /// Gets the default exchange rate provider for CZK currency.
        /// </summary>
        /// <returns>The default exchange rate provider for CZK currency.</returns>
        public string GetCZKProvider() => this["CZK"];
    }
}
