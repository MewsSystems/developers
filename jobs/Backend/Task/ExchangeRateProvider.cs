using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, string languageCode)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();

            string currentUtcDateString = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

            ExchangeRateApiResponse exchangeRateApiResponse = await httpClient.GetFromJsonAsync<ExchangeRateApiResponse>(
        $"https://api.cnb.cz/cnbapi/exrates/daily?date={currentUtcDateString}&lang={languageCode}");

            List<ExchangeRateApiData> filteredExchangeRateData =
                exchangeRateApiResponse.Rates.Where(x => currencies.Any(y => y.Code == x.CurrencyCode)).ToList();

            var exchangeRateData = new List<ExchangeRate>();

            foreach (ExchangeRateApiData data in filteredExchangeRateData)
            {
                exchangeRateData.Add(new ExchangeRate(currencies.FirstOrDefault(x => x.Code == data.CurrencyCode), currencies.FirstOrDefault(x => x.Code == "CZK"), data.Rate));
            }

            return exchangeRateData;
        }
    }
}
