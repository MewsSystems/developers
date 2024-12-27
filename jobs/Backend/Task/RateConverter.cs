using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    internal static class RateConverter
    {
        // Convert from ExchangeRateApi to ExchangeRate
        public static IEnumerable<ExchangeRate> ConvertApiToExchangeRate(IEnumerable<ExchangeRateApi> ratesApi, Currency baseCurrency)
        {
            return ratesApi.Select(c => new ExchangeRate(new Currency(c.CurrencyCode), baseCurrency, c.Rate / c.Amount));
        }
    }
}
