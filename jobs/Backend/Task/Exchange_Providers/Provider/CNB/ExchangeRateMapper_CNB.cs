using ExchangeRateUpdater.Exchange_Providers.Interfaces;
using ExchangeRateUpdater.Exchange_Providers.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace ExchangeRateUpdater.Exchange_Providers.Provider.CNB
{
    /// <summary>
    /// Provides a mapper for CNB (Czech National Bank) exchange rate data to a common ExchangeRate object.
    /// </summary>
    internal class ExchangeRateMapper_CNB : IExchangeRateMapper<CNB_Exchange_Rate>
    {
        /// <summary>
        /// Maps a CNB exchange rate data object to a common ExchangeRate object.
        /// </summary>
        /// <param name="exchangeRate">The CNB exchange rate data to be mapped.</param>
        /// <returns>An ExchangeRate object containing the mapped exchange rate information.</returns>
        public ExchangeRate Map(CNB_Exchange_Rate exchangeRate)
        {
            if (exchangeRate == null)
            {
                throw new ArgumentNullException(nameof(exchangeRate));
            }
            var sourceCurrency = new Currency(exchangeRate.CurrencyCode);
            var targetCurrency = new Currency("CZK");

            var value = (decimal)exchangeRate.Rate / exchangeRate.Amount;

            return new ExchangeRate(sourceCurrency, targetCurrency, value);
        }

    }
}
