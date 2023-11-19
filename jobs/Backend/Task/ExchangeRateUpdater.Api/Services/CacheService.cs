using ExchangeRateUpdater.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ExchangeRateUpdater.Api.Services
{
    public interface ICacheService
    {
        Task AddDailyExchangeRatesAsync(CnbDailyExchangeRatesResponse dailyExchangeRates, CancellationToken cancellationToken);
        Task<CnbDailyExchangeRatesResponse?> GetDailyExchangeRatesAsync(CancellationToken cancellationToken);
    }

    public class CacheService: ICacheService
    {
        private readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(90), // in real-life scenario these values would be different
            SlidingExpiration = TimeSpan.FromSeconds(60)
        };

        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task AddDailyExchangeRatesAsync(
            CnbDailyExchangeRatesResponse dailyExchangeRates,
            CancellationToken cancellationToken)
        {
            var serializedData = JsonSerializer.Serialize(dailyExchangeRates);
            await _distributedCache.SetStringAsync(GetCacheKey(), serializedData, _cacheOptions, cancellationToken);
        }

        public async Task<CnbDailyExchangeRatesResponse?> GetDailyExchangeRatesAsync(CancellationToken cancellationToken)
        {
            var serializedData = await _distributedCache.GetStringAsync(GetCacheKey());
            if (string.IsNullOrEmpty(serializedData))
            {
                return null;
            }

            var exchangeRates = JsonSerializer.Deserialize<CnbDailyExchangeRatesResponse>(serializedData);
            return exchangeRates;
        }

        private static string GetCacheKey()
        {
            return $"cnb-daily-exchange-rates-{DateTime.UtcNow.ToString("dd-MM-yyyy")}";
        }
    }
}