using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
    /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public class ExchangeRateProvider
    {
        //protected static readonly char[] TableName = {'A', 'B'};

        private readonly string _sourceUrLs = $"http://api.nbp.pl/api/exchangerates/tables/a/today/";
        private const string SourceCurrency = "PLN";
        /// <summary>
        /// National Bank of Poland provides rates in several tables with PLN as a source currency.
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var array = currencies as Currency[];
            var currenciesArray = array ?? currencies.ToArray();

            if (NoSourceCurrencyIn(currenciesArray))
                return Enumerable.Empty<ExchangeRate>();

            var sourceData = GetRates();

            return sourceData
                .Select(r => new ExchangeRate(new Currency(SourceCurrency), new Currency(r.Code), r.Value))
                .Where(o => currenciesArray.Contains(o.TargetCurrency));
        }

        private static bool NoSourceCurrencyIn(IEnumerable<Currency> currencies) 
                              => currencies.All(c => c.Code != SourceCurrency);

        private IEnumerable<Rate> GetRates()
        {
            RatesTableDto[] data;

            var request = WebRequest.CreateHttp(_sourceUrLs);
            using (var response = request.GetResponse())
            {

                var serializer = new DataContractJsonSerializer(typeof(RatesTableDto[]));

                using (var stream = response.GetResponseStream())
                {
                    data = (RatesTableDto[])serializer.ReadObject(stream);
                }

            }

            return data[0].Rates;
        }
    }
}


