using ExchangeRateUpdater.Common.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            HashSet<string> currencyFilter = currencies.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            List<ExchangeRate> exchangeRates = new();

            try
            {
                using (HttpClient httpClient = new())
                {
                    CancellationTokenSource cts = new CancellationTokenSource(5000);
                    HttpResponseMessage response = await httpClient.GetAsync(ErpPlatform.BaseUrl, cts.Token);

                    response.EnsureSuccessStatusCode();

                    using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                    {
                        await reader.ReadLineAsync();
                        await reader.ReadLineAsync();

                        string exchangeRate;

                        while ((exchangeRate = await reader.ReadLineAsync()) != null)
                        {
                            var columns = exchangeRate.Split('|');
                            if (columns.Length != 5) continue;

                            var targetCode = columns[3];
                            if (!currencyFilter.Contains(targetCode))
                                continue;

                            var sourceCurrency = new Currency(ErpPlatform.DefaultSourceCurrency);
                            var targetCurrency = new Currency(targetCode);
                            var amount = int.Parse(columns[2]);
                            var value = decimal.Parse(columns[4], CultureInfo.InvariantCulture);

                            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, amount, value));
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("The request was cancelled due to timeout.");
            }
            catch (Exception)
            {
                throw;
            }

            return exchangeRates;
        }
    }
}
