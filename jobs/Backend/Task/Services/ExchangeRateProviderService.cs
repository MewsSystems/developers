using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IEnumerable<IExhangeRateFetcher> _fetchers;
        private readonly IExchangeRateParser _parser;
        private readonly ILogger<ExchangeRateProviderService> _logger;
        private readonly IMemoryCache _cache;
        private const string CacheKey = nameof(CacheKey);
        private const string SourceCurrency = "CZK";

        public ExchangeRateProviderService(
        IEnumerable<IExhangeRateFetcher> fetchers,
        IExchangeRateParser parser,
        IMemoryCache cache,
        ILogger<ExchangeRateProviderService> logger)
        {
            _fetchers = fetchers;
            _parser = parser;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies)
        {
            var targetCodes = new HashSet<string>(
                currencies.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase
            );

            
            if (_cache.TryGetValue(CacheKey, out List<ExchangeRate> cachedRates))
            {
                _logger.LogInformation("Returning filtered exchange rates from cache.");
                return cachedRates
                    .Where(rate => targetCodes.Contains(rate.TargetCurrency.Code))
                    .ToList();
            }

            // Parallel fetch
            var fetchTasks = _fetchers.Select(s => s.FetchAsync()).ToList();

            string[] content;
            try
            {
                content = await Task.WhenAll(fetchTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "One or more sources failed during fetch.");
                // If we have fallback cache, return that:
                if (_cache.TryGetValue(CacheKey, out List<ExchangeRate> fallback))
                {
                    _logger.LogWarning("Returning cached rates due to fetch failure.");
                    return fallback.Where(rate => targetCodes.Contains(rate.TargetCurrency.Code)).ToList();
                }

                throw new ApplicationException("Failed to fetch exchange rates and no cached fallback is available.", ex);
            }

            var exchangeRates = new List<ExchangeRate>();
            foreach (var raw in content)
            {
                exchangeRates.AddRange(_parser.Parse(raw, currencies, new Currency(SourceCurrency)));
            }



            var now = DateTimeOffset.UtcNow;
            var nextUpdate = new DateTimeOffset(now.Date.AddHours(13.5)); // 2:30 PM CET Daily update according to their website
            if (now >= nextUpdate) nextUpdate = nextUpdate.AddDays(1);

            _cache.Set(CacheKey, exchangeRates, nextUpdate);

            _logger.LogInformation("Successfully fetched exchange rates until {NextUpdate}.", nextUpdate);
            return exchangeRates
               .Where(rate => targetCodes.Contains(rate.TargetCurrency.Code))
               .ToList();
        }
    }
}
