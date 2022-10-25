using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ExchangeRateUpdater.Caching
{
    /// <summary>
    /// This is a simplest caching solution - expire cached values after a specified amount of time.
    /// The expiration period could be in the order of a few minutes.
    /// 
    /// If the cache is used under intensive load (multiple threads accessing at the same time, most of the time),
    /// then add synchronization via Monitor to ensure that only one thread refreshes the cache while others wait.
    /// 
    /// If latency is an absolute priority, then cache refresh should happen periodically in a background thread.
    /// 
    /// If availability of exchange rate source (cnb.cz) is a concern, then the cache entry should not be expired
    /// until new data is fetched. Also better to have a backup copy stored in a database.
    /// </summary>
    public class ExchangeRateCache : IExchangeRateCache
    {
        private static readonly object _cacheKey = new();

        private readonly IMemoryCache _memoryCache;
        private readonly Options _options;

        public ExchangeRateCache(IMemoryCache memoryCache, Options options)
        {
            _memoryCache = memoryCache;
            _options = options;

            if (_options.ExpirationPeriod <= TimeSpan.Zero)
            {
                throw new Exception("Cache expiration period is not set");
            }
        }

        public ExchangeRate[]? GetValue()
        {
            return _memoryCache.TryGetValue(_cacheKey, out ExchangeRate[] value) ? value : null;
        }

        public void Set(ExchangeRate[] exchangeRates)
        {
            _memoryCache.Set(_cacheKey, exchangeRates, _options.ExpirationPeriod);
        }

        public class Options
        {
            public TimeSpan ExpirationPeriod { get; set; }
        }
    }
}
