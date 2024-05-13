using ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi;
using ExchangeRateUpdater.ExchangeRateAPI.DTOs;
using ExchangeRateUpdater.ExchangeRateAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateAPI.ExchangeRateProvider
{
    public class CBNExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICBNClientApi _apiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CBNExchangeRateProvider> _logger;

        private const string ExRatesCacheKey = "ExRatesDaily";

        public CBNExchangeRateProvider(ICBNClientApi apiClient, IMemoryCache memoryCache, IConfiguration configuration, ILogger<CBNExchangeRateProvider> logger)
        {
            _apiClient = apiClient;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (!_memoryCache.TryGetValue(ExRatesCacheKey, out ExchangeRatesResponseDTO exchangeRatesResponse))
            {
                exchangeRatesResponse = await _apiClient.GetExratesDaily();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromSeconds(_configuration.GetValue<double>("CacheDurationInSeconds")));

                _memoryCache.Set(ExRatesCacheKey, exchangeRatesResponse, cacheEntryOptions);

                _logger.LogInformation($"{ExRatesCacheKey} cached.");
            }

            var targetCurrency = new Currency("CZK");

            var results = exchangeRatesResponse.Rates
                .Where(x => currencies.Select(c => c.Code.ToUpper()).Contains(x.CurrencyCode.ToUpper()))
                .Select(x => new ExchangeRate(new Currency(x.CurrencyCode), targetCurrency, x.Rate / x.Amount))
                .ToList();

            // Skipping invalid passed currencies because "If the source does not provide some of the currencies, ignore them."

            _logger.LogInformation($"API returned currencies: {string.Join(",", results?.Select(x => x.SourceCurrency).ToList())}.");

            return results;
        }
    }
}
