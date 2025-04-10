using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Extensions
{
    public static class CNBRatesExtensions
    {
        /// <summary>
        /// Method maps CNBRates object into an a collection of ExchangeRate objects. Specifying the source currency as "CZK"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<ExchangeRate> ToExchangeRates(this CNBRates data)
        {
            return data.Rates?.Select(x => new ExchangeRate(new Currency("CZK"), new Currency(x.CurrencyCode), x.Rate / x.Amount)) ?? Enumerable.Empty<ExchangeRate>();
        }
    }
}
