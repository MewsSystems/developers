using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Services.CurrencySource
{
    public class CurrencySourceGetService : BaseCacheService, ICurrencySourceGetService
    {
        private readonly ILogger<CurrencySourceGetService> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly ICurrencySourceRepository _currencySourceRepository;

        public CurrencySourceGetService(ILogger<CurrencySourceGetService> logger, IDistributedCache distributedCache, ICurrencySourceRepository currencySourceRepository)
            :base("CurrencySource", 5, 2)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _currencySourceRepository = currencySourceRepository;
        }

        public async Task<List<CurrencySourceResponse>> GetAllCurrencySources()
        {
            _logger.LogInformation("CurrencySourceGetService - GetAllCurrencySources called");
            var cachedData = await _distributedCache.GetAsync(CacheKey);
            if (IsCacheEmpty(cachedData))
            {
                _logger.LogInformation("CurrencySourceGetService - GetAllCurrencySources - Nothing found in cache.");
                
                var currencySources = await _currencySourceRepository.GetCurrencySourcesAsync();
                _logger.LogInformation("CurrencySourceGetService - currency source repository returned {CurrencySources} results", currencySources.Count());

                var sources = currencySources.Select(x => x.ToCurrencySourceResponse()).ToList();

                _logger.LogInformation("CurrencySourceGetService - GetAllCurrencySources - Adding currency sources to cache");
                await _distributedCache.SetAsync(CacheKey, ConvertListToByteArray(sources), CacheOptions);

                return sources;
            }
            else
            {
                _logger.LogInformation("CurrencySourceGetService - GetAllCurrencySources - Cached data found.");

                var sourceCurrencies = ConvertByteArrayToList<CurrencySourceResponse>(cachedData);
                return sourceCurrencies;
            }
        }
    }
}
