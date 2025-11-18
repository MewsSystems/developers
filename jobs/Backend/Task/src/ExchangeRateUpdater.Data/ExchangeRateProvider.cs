using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.Data.Helpers;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Data
{
    /// <summary>
    /// Exchange rate provider that fetches rates from CNB (Czech National Bank).
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private const string DailyRatesUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        public ExchangeRateProvider(HttpClient httpClient, ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Synchronous wrapper around async operation and I am now using ConfigureAwait(false) to prevent deadlock 
            var result = GetExchangeRatesAsync(currencies).ConfigureAwait(false).GetAwaiter().GetResult();
            return result;
        }

        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            try
            {
                _logger.LogDebug("Downloading daily exchange rates from {Url}", DailyRatesUrl);

                using var res = await _httpClient.GetAsync(DailyRatesUrl, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                res.EnsureSuccessStatusCode();
                await using var stream = await res.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var sr = new StreamReader(stream);

                var allText = await sr.ReadToEndAsync().ConfigureAwait(false);

                // Parser is now a static helper
                return ParseHelper.ParseRates(allText, currencies);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Failed to download exchange rates from {Url}. HTTP error occurred.", DailyRatesUrl);
                throw;
            }
            catch (IOException ioEx)
            {
                _logger.LogError(ioEx, "I/O error occurred while reading exchange rate data.");
                throw;
            }
            catch (OperationCanceledException cancelEx)
            {
                _logger.LogWarning(cancelEx, "Exchange rate download was cancelled.");
                throw;
            }
        }
    }
}
