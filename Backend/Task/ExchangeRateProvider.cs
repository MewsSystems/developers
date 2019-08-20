using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string ExchangeRatesUrlKey = "Cnb.ExchageRatesUrl";
        private const string AdditionalExchangeRateUrlKey = "Cnb.ExchageRatesUrl.Other";

        private static HttpClient _httpClient = new HttpClient();
        private IExchangeRateParser _parser = new CnbExchangeRateParser();

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var loadTasks = new Task<IEnumerable<ExchangeRate>>[] {
                this.LoadFromUrlAsync(ConfigurationManager.AppSettings[ExchangeRateProvider.ExchangeRatesUrlKey]),
                this.LoadFromUrlAsync(ConfigurationManager.AppSettings[ExchangeRateProvider.AdditionalExchangeRateUrlKey])
            };
            Task.WaitAll(loadTasks);

            var rates = loadTasks.SelectMany(o => o.Result).Where(o => currencies.Contains(o.TargetCurrency)).ToList();
            return rates;
        }

        private async Task<IEnumerable<ExchangeRate>> LoadFromUrlAsync(string url) {
            using (var response = await _httpClient.GetAsync(url).ConfigureAwait(false)) {
                response.EnsureSuccessStatusCode();

                using (var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false)) {
                    return await _parser.ParseAsync(responseContent);
                }
            }
        }
    }
}
