using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.cnb.cz/cnbapi/exrates/daily?date=2023-06-27&lang=EN");
            using var response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStream();
            var exchangeRates = JsonSerializer.Deserialize<CnbExchangeRates>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var result = new List<ExchangeRate>();
            var sourceCurrency = new Currency("CZK");
            foreach (var rate in exchangeRates.Rates)
            {
                if (currencies.Any(x => x.Code == rate.CurrencyCode))
                {
                    var targetCurrency = new Currency(rate.CurrencyCode);
                    var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, rate.Rate);
                    result.Add(exchangeRate);
                }
            }

            return result;
        }
    }
}
