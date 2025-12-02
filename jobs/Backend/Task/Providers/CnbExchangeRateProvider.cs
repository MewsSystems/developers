using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const string DailyRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/" +
            "central-bank-exchange-rate-fixing/daily.txt";

        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var raw = DownloadDailyRatesAsync().GetAwaiter().GetResult();

            return Enumerable.Empty<ExchangeRate>();
        }


        /// <summary>
        /// Downloads the daily exchange rates TXT file from the CNB public endpoint.
        /// </summary>
        /// <exception cref="HttpRequestException">
        /// Thrown when the CNB endpoint is unavailable or returns an invalid response.
        /// </exception>
        private async Task<string> DownloadDailyRatesAsync()
        {
            try
            {
                using var response = await HttpClient.GetAsync(DailyRatesUrl);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                    throw new InvalidOperationException("Received empty response from CNB endpoint.");

                return content;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Failed to download daily exchange rates from CNB.", ex);
            }
        }
    }
}
