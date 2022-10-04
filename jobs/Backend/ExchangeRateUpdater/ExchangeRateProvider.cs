using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string CZK_CODE = "CZK";
        private Currency targetCurrency = new Currency(CZK_CODE);

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = Enumerable.Empty<ExchangeRate>();
            var calculatedRates = new List<ExchangeRate>();

            try
            {
                var rateData = await GetRates();

                var parsedRates = ParseRates(rateData);

                foreach (ExchangeRateItem cnbData in parsedRates)
                {
                    calculatedRates.Add(new ExchangeRate(
                        new Currency(cnbData.Code),
                        targetCurrency,
                        cnbData.Rate / cnbData.Amount
                        ));
                }

                rates = calculatedRates.Where(rates => currencies.Any(currency => currency.Code.Equals(rates.SourceCurrency.Code)));
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return rates;
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
        /// Helper method to parse rate data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IEnumerable<ExchangeRateItem> ParseRates(string data)
        {
            var _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "|",
                HasHeaderRecord = true
            };

            // ensure input is valid, exception handling should be done by calling code
            if (data == null || string.IsNullOrWhiteSpace(data) || data.Length <= 0)
            {
                throw new ArgumentNullException(nameof(data), "String input is required");
            }

            var textReader = new StringReader(data);
            using var csv = new CsvReader(textReader, _csvConfiguration);
            // skip the first line because it isn't used
            csv.Read();
            return csv.GetRecords<ExchangeRateItem>().ToList();
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
