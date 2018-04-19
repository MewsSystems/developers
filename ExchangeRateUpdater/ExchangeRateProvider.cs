using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.CNB;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// CNB Exchange rate provider
    /// </summary>
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// <param name="targetCurrencies">The target currencies.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> targetCurrencies)
        {
            if (targetCurrencies == null || targetCurrencies.Count() == 0) throw new ArgumentNullException();

            // get data
            var stream = default(Stream);
            using (var CNBClient = new CNBClient())
            {
                stream = await CNBClient.GetDataAsync();
            }

            // parse data
            var parser = new CNBParser(targetCurrencies);
            var result = await parser.Parse(stream);

            return result;
        }
    }
}
