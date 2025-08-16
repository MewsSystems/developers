using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                try
                {
                    rates.Add(ExchangeRateCache.GetExchangeRateAsync(currency.Code).Result);
                }
                catch (Exception e)
                {
                    // TODO: proper logging
                    Console.WriteLine($"Exception while fetching FX Rates: {e.Message}");
                }
            }
            return rates;
        }
    }
}
