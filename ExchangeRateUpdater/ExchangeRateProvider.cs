using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

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
        private const string SourceCurrency = "PLN";
        /// <summary>
        /// National Bank of Poland provides rates thru API in two tables ("A" and "B") with PLN as a source currency.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currenciesArray = currencies as Currency[] ?? currencies.ToArray();

            if (NoSourceCurrencyIn(currenciesArray)) return Enumerable.Empty<ExchangeRate>();

            var sourceData = GetRates('a');

            try //to get rates for currencies that are maybe distinct in table "A" 
            {
                sourceData = sourceData.Union(GetRates('b'));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't get rates from table 'B', service unavailable due to: {ex.Message}");
            }


            return sourceData?
                   .Select(r => new ExchangeRate(new Currency(SourceCurrency), new Currency(r.Code), r.Value))
                   .Where(c => currenciesArray.Contains(c.TargetCurrency));
        }

        private static bool NoSourceCurrencyIn(IEnumerable<Currency> currencies)
                              => currencies.All(c => c.Code != SourceCurrency);

        private IEnumerable<Rate> GetRates(char tableName)
        {
            RatesTableDto[] data;
            string _sourceEndpoint = $"http://api.nbp.pl/api/exchangerates/tables/{tableName}";

            var request = WebRequest.CreateHttp(_sourceEndpoint);//API call

            using (var response = request.GetResponse())
            {

                var serializer = new DataContractJsonSerializer(typeof(RatesTableDto[]));

                using (var stream = response.GetResponseStream())
                {
                    data = (RatesTableDto[])serializer.ReadObject(stream);
                }

            }

            return data[0]?.Rates;
        }
    }
}


