using System.Collections.Generic;
using System.IO;
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

        private const string SOURCE_URL = "http://www.norges-bank.no/api/Currencies?frequency=d2&language=en&observationlimit=1&returnsdmx=false";
        private const string SOURCE_CURRENCY = "NOK";

        /// <summary>
        /// Norwegian National Bank provides only rates with NOK currency as a source currency.
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesArray = currencies as Currency[] ?? currencies.ToArray();

            if (SourceCurrencyNotRequested(currenciesArray))
                return Enumerable.Empty<ExchangeRate>();

            var sourceData = GetWebSource();

            return sourceData
                .Select(s => new ExchangeRate(new Currency(SOURCE_CURRENCY), new Currency(s.Id), s.CurrentValue))
                .Where(x => currenciesArray.Contains(x.TargetCurrency));
        }

        private static bool SourceCurrencyNotRequested(IEnumerable<Currency> currencies)
        {
            return currencies.All(c => c.Code != SOURCE_CURRENCY);
        }

        private static NorgesExchangeRate[] GetWebSource()
        {
            NorgesExchangeRate[] content;

            var request = WebRequest.Create(SOURCE_URL);
            var response = request.GetResponse();
            var serializer = new DataContractJsonSerializer(typeof(NorgesExchangeRate[]));

            using (var stream = response.GetResponseStream())
            {
                content = (NorgesExchangeRate[]) serializer.ReadObject(stream);
            }

            response.Close();

            return content;
        }
    }
}
