using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExchangeRateUpdater.Models;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, Currency baseCurrency)
        {
            var exchangeRates = new List<ExchangeRate>();

            // Created two different calls to exemplify how API can change individually, despite being right now, returning the data in the same format
            var ratesDaily = GetExchangeRatesDailyAsync(currencies).Result;
            var ratesForex = GetExchangeRatesForexAsync(currencies).Result;

            // Conversion should be one or two different depending on what is done with the API calls. In this case, for simplicity, I've done one single converter
            exchangeRates.AddRange(RateConverter.ConvertApiToExchangeRate(ratesDaily, baseCurrency));
            exchangeRates.AddRange(RateConverter.ConvertApiToExchangeRate(ratesForex, baseCurrency));

            return exchangeRates;
        }

        private async Task<IEnumerable<ExchangeRateApi>> GetExchangeRatesDailyAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRateApi>();

            var dayDate = DateTime.Now.ToString("yyyy-MM-dd");

            var urlApi = $"https://api.cnb.cz/cnbapi/exrates/daily";
            var parametersApi = $"?date={dayDate}";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApi);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(parametersApi).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var ratesList = JsonConvert.DeserializeObject<ExchangeRatesList>(jsonString);

                if (ratesList != null)
                {
                    exchangeRates.AddRange(ratesList.Rates.Where(e => currencies.Any(c => c.Code == e.CurrencyCode)));
                }
            }

            return exchangeRates;
        }

        private async Task<IEnumerable<ExchangeRateApi>> GetExchangeRatesForexAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRateApi>();

            var monthDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");

            var urlApi = $"https://api.cnb.cz/cnbapi/fxrates/daily-month";
            var parametersApi = $"?yearMonth={monthDate}";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApi);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(parametersApi).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var ratesList = JsonConvert.DeserializeObject<ExchangeRatesList>(jsonString);

                if (ratesList != null)
                {
                    exchangeRates.AddRange(ratesList.Rates.Where(e => currencies.Any(c => c.Code == e.CurrencyCode)));
                }
            }

            return exchangeRates;
        }
    }
}
