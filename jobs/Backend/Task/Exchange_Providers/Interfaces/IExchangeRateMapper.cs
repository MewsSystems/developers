using ExchangeRateUpdater.Exchange_Providers.Models;
using ExchangeRateUpdater.Exchange_Providers.Provider.CNB;

namespace ExchangeRateUpdater.Exchange_Providers.Interfaces
{
    internal interface IExchangeRateMapper<T>
    {
        /// <summary>
        /// Maps an exchange rate data object to a common ExchangeRate object.
        /// </summary>
        /// <param name="exchangeRate">The exchange rate data to be mapped.</param>
        /// <returns>An ExchangeRate object containing the mapped exchange rate information.</returns>
        ExchangeRate Map(T exchangeRate);
    }
}
