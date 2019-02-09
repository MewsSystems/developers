using ExchangeRateUpdater.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            foreach (var curency in currencies)
            {
                var rate = ExchangeRateCacher.Instance.GetExchangeRate(curency.Code);
                if (rate != null)
                {
                    rates.Add(rate);
                }
            }

            return rates.AsEnumerable(); ;
        }

    }
    
}
