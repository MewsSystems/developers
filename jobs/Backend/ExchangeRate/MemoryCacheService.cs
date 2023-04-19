using CzechNationalBankClient.Model;
using ExchangeRateProvider.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly int _expireTimeInSeconds;

        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _memoryCache = memoryCache;
            _expireTimeInSeconds = cacheSettings?.Value?.ExpireTimeInSeconds ?? 30;
        }

        public IEnumerable<CnbExchangeRate>? GetCachedRatesValue(string key)
        {
            _memoryCache.TryGetValue(key, out IEnumerable<CnbExchangeRate> rates);
            return rates;
        }

        public void SetCachedRates(string key, IEnumerable<CnbExchangeRate> rates)
        {
            _memoryCache.Set(key, rates, TimeSpan.FromSeconds(_expireTimeInSeconds));
        }
    }
}
