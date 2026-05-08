using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Cache
{
    public class CnbRatesCache : ICnbRatesCache
    {
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;
        private readonly Func<DateTimeOffset> _expirationFactory;
        private readonly ILogger<CnbRatesCache> _logger;

        public CnbRatesCache(IMemoryCache cache, string cacheKey, Func<DateTimeOffset> expirationFactory, ILogger<CnbRatesCache> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cacheKey = cacheKey ?? throw new ArgumentNullException(nameof(cacheKey));
            _expirationFactory = expirationFactory ?? throw new ArgumentNullException(nameof(expirationFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Dictionary<string, decimal>> GetOrCreateAsync(Func<Task<Dictionary<string, decimal>>> factory)
        {
            if (_cache.TryGetValue(_cacheKey, out Dictionary<string, decimal> cachedValue))
            {
                _logger.LogInformation("Cache hit for key {CacheKey}", _cacheKey);
                return cachedValue;
            }

            _logger.LogInformation("Cache miss for key {CacheKey}. Fetching new data...", _cacheKey);
            try
            {
                var value = await factory();
                _cache.Set(_cacheKey, value, _expirationFactory());
                _logger.LogInformation("Cached new value for key {CacheKey}", _cacheKey);
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}