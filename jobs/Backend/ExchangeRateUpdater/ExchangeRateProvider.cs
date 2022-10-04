using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            await GetRates();

            return Enumerable.Empty<ExchangeRate>();
        }

        /// <summary>
        /// Helper method to fetch the rates from the source
        /// TODO: Improve strucutre
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRates()
        {
            TimeSpan? timeout = null;
            string responseContent = string.Empty;

            using (var httpClient = new HttpClient())
            {
                // TODO: Verify Source(s) - using daily for initial POC
                var rateSource = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

                HttpResponseMessage responseMessage = await httpClient.GetAsync(rateSource, GetCancellationTokenFromTimeout(timeout)).ConfigureAwait(false);
                responseMessage.EnsureSuccessStatusCode();

                responseContent = await responseMessage.Content.ReadAsStringAsync();
            }

            return responseContent;
        }

        /// <summary>
        /// Helper method to setup a CancellationToken (from a custom timeout) for the http request
        /// </summary>
        /// <param name="timeout">Optional, custom timeout</param>
        /// <returns>CancellationToken</returns>
        private CancellationToken GetCancellationTokenFromTimeout(TimeSpan? timeout)
        {
            // set default timeout if not supplied
            if (timeout.HasValue == false)
            {
                timeout = TimeSpan.FromSeconds(30);
            }

            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout.Value);

            return cts.Token;
        }
    }
}
