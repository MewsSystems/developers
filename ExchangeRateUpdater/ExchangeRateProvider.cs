using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        string TargetCurrency = "EUR";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string query = "";
            List<ExchangeRate> output = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                query += currency.Code + TargetCurrency + ",";
            }
            using (var client = new WebClient())
            {
                var json = client.DownloadString("https://query.yahooapis.com/v1/public/yql?q=select+*+from+yahoo.finance.xchange+where+pair+=+%22"
                    + query + "%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=");
                dynamic rates = JsonConvert.DeserializeObject<dynamic>(json);
                for (int i = 0; i < currencies.Count(); i++)
                {
                    dynamic a = rates.query.results.rate[i];
                    if (a.Rate != "N/A")
                    {
                        output.Add(new ExchangeRate(new Currency(((string)a.id).Substring(0, 3)),
                            new Currency(TargetCurrency),
                            (decimal)a.Rate));
                    }
                }
            }

            return output;
        }
    }
}
