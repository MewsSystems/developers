using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly string _url;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public ExchangeRateProvider(string url)
        {
            _url = url;
        }
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IList<ExchangeRate> exchangeRates = new List<ExchangeRate>(currencies.Count() * currencies.Count());
            HttpClient client = new HttpClient();

            foreach (var target in currencies)
            {
                var values = GetValues(client, target.Code);

                foreach (var source in currencies)
                    if (values.ContainsKey(source.Code))
                        exchangeRates.Add(new ExchangeRate(source, target, values[source.Code]));
            }

            return exchangeRates;
        }

        private Dictionary<string, decimal> GetValues(HttpClient client, string baseCode)
        {
            var response = client.GetAsync($"{_url}?base={baseCode}").Result;
            if (!response.IsSuccessStatusCode)
                return new Dictionary<string, decimal>(0);

            var responseString = response.Content.ReadAsStringAsync().Result;
            JObject responseObject = JObject.Parse(responseString);

            var rates = responseObject["rates"];
            if (!rates.HasValues)
                return new Dictionary<string, decimal>(0);

            return rates.ToObject<Dictionary<string, decimal>>();
        }
    }
}
