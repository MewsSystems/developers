using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

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

        private string sourceURL = "http://api.nbp.pl/api/exchangerates/tables/a/today/";
        private const string sourceCurrency = "PLN";
        /// <summary>
        /// National Bank of Poland provides rates to PLN as a source currency.
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesArray = currencies as Currency[] ?? currencies.ToArray();

            if (AllButSourceCurrency(currenciesArray))
                return Enumerable.Empty<ExchangeRate>();

            var sourceData = GetRates();

            return sourceData
                .Select(r => new ExchangeRate(new Currency(sourceCurrency), new Currency(r.code), r.mid))
                .Where(o => currenciesArray.Contains(o.TargetCurrency));
        }

        private static bool AllButSourceCurrency(IEnumerable<Currency> currencies)
        {
            return currencies.All(c => c.Code != sourceCurrency);
        }

        private Rate[] GetRates()
        {
            RatesTable[] data;

            var request = WebRequest.CreateHttp(sourceURL);
            using (var response = request.GetResponse())
            {

                var serializer = new DataContractJsonSerializer(typeof(RatesTable[]));

                using (var stream = response.GetResponseStream())
                {
                    data = (RatesTable[])serializer.ReadObject(stream);
                }

            }

            return data[0].rates;
        }
    }
}


