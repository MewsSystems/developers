using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Helpers
{
    public static class ExchangeRateHelper
    {
        public static List<ExchangeRate> ConvertToExchangeRates(List<ThirdPartyExchangeRate> thirdPartyExchangeRates, Currency sourceCurrency)
        {
            if (thirdPartyExchangeRates == null || thirdPartyExchangeRates.Count == 0)
            {
                // If the input is null or empty, then we will want to handle this
                return null;
            }

            var exchangeRates = new List<ExchangeRate>();

            foreach(var thirdPartyExchangeRate in thirdPartyExchangeRates)
            {
                var targetCurrency = new Currency(thirdPartyExchangeRate.Code);
                var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, thirdPartyExchangeRate.Rate);
                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }
    }
}
