using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateProviderService> _logger;
        private readonly string _dailyRatesUrl;
        private readonly IMemoryCache _cache;
        private readonly string _otherCurrencyRatesUrl;
        private const string CacheKey = nameof(CacheKey);
        private const string SourceCurrency = "CZK";

        public ExchangeRateProviderService(
            HttpClient httpClient,
            ILogger<ExchangeRateProviderService> logger,
            IOptions<CzechBankSettings> options,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _dailyRatesUrl = options.Value.DailyRatesUrl ?? throw new ArgumentNullException(nameof(options.Value.DailyRatesUrl));
            _otherCurrencyRatesUrl = options.Value.OtherCurrencyRatesUrl ?? throw new ArgumentNullException(nameof(options.Value.OtherCurrencyRatesUrl));
            _cache = cache;
        }

        public async Task<List<ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies)
        {
            if (_cache.TryGetValue(CacheKey, out List<ExchangeRate> cachedRates))
            {
                _logger.LogInformation("Returning filtered exchange rates from cache.");
                return cachedRates.Where(rate => currencies.Contains(rate.TargetCurrency.Code)).ToList();
            }

            try
            {
                _logger.LogInformation("Getting exchange rates from Czech National Bank txt source");

                var exchangeRates = new List<ExchangeRate>();
                var sourceCurrency = new Currency(SourceCurrency);

                var dailyTask = _httpClient.GetStringAsync(_dailyRatesUrl);
                var otherTask = _httpClient.GetStringAsync(_otherCurrencyRatesUrl);

                await Task.WhenAll(dailyTask, otherTask);

                exchangeRates.AddRange(ParseRatesFromContent(dailyTask.Result, currencies, sourceCurrency));
                exchangeRates.AddRange(ParseRatesFromContent(otherTask.Result, currencies, sourceCurrency));


                var now = DateTimeOffset.UtcNow;
                var nextUpdate = new DateTimeOffset(now.Date.AddHours(13.5)); // 2:30 PM CET Daily update according to their website
                if (now >= nextUpdate) nextUpdate = nextUpdate.AddDays(1);

                _cache.Set(CacheKey, exchangeRates, nextUpdate);

                _logger.LogInformation("Successfully fetched exchange rates until {NextUpdate}.", nextUpdate);
                return exchangeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch exchange rates from CNB.");

                if (_cache.TryGetValue(CacheKey, out List<ExchangeRate> fallbackRates))
                {
                    _logger.LogWarning("Returning previously cached exchange rates due to error.");
                    return fallbackRates
                        .Where(rate => currencies.Contains(rate.TargetCurrency.Code))
                        .ToList();
                }

                _logger.LogCritical("No cached data available. Unable to fulfill request.");
                throw new ApplicationException("Unable to fetch exchange rates and no fallback data available.", ex);
            }
        }

        private static List<ExchangeRate> ParseRatesFromContent(string content, IEnumerable<Currency> targetCodes, Currency sourceCurrency)
        {
            var rates = new List<ExchangeRate>();
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++) // Skip header + date
            {
                var parts = lines[i].Split('|');
                int expectedColumns = 5;
                if (parts.Length < expectedColumns) continue;

                string code = parts[3].Trim();
                if (!targetCodes.Contains(code)) continue;

                int amount = int.Parse(parts[2].Trim());
                decimal rate = decimal.Parse(parts[4].Trim().Replace(',', '.'), CultureInfo.InvariantCulture); //Czech bank uses , instead of .
                decimal normalized = rate / amount;

                rates.Add(new ExchangeRate(sourceCurrency, new Currency(code), normalized));
            }

            return rates;
        }
    }
}
