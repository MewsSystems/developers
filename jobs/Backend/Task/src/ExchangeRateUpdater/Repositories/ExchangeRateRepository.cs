using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Repositories;

public class ExchangeRateRepository(
    IMemoryCache memoryCache,
    ICnbExchangeRateClient cnbExchangeRateClient,
    ILogger<ExchangeRateRepository> logger
) : IExchangeRateRepository
{
    public async Task<IEnumerable<ExchangeRate>> GetCzkExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<ExchangeRate> commonRates = await GetRatesFromCacheOrFetchAsync(
            ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey,
            cnbExchangeRateClient.FetchCommonExchangeRatesAsync,
            ExchangeRateCacheHelper.Cnb.CommonRates.GetExpiry(),
            cancellationToken
        );

        IEnumerable<ExchangeRate> uncommonRates = await GetRatesFromCacheOrFetchAsync(
            ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey,
            cnbExchangeRateClient.FetchUncommonExchangeRatesAsync,
            ExchangeRateCacheHelper.Cnb.UncommonRates.GetExpiry(),
            cancellationToken
        );

        IEnumerable<ExchangeRate> rates = commonRates.Concat(uncommonRates);
        return rates;
    }

    internal async Task<IEnumerable<ExchangeRate>> GetRatesFromCacheOrFetchAsync(string cacheKey, Func<CancellationToken, Task<IEnumerable<ExchangeRate>>> fetchMethod, DateTimeOffset absoluteExpiration, CancellationToken cancellationToken)
    {
        bool cachedRatesFound = memoryCache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate>? cachedRates);
        if (cachedRatesFound && cachedRates is not null)
        {
            logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedRates;
        }

        try
        {
            logger.LogInformation("Cache miss for {CacheKey}, fetching from external service", cacheKey);

            IEnumerable<ExchangeRate> exchangeRates = await fetchMethod(cancellationToken);

            memoryCache.Set(cacheKey, exchangeRates, absoluteExpiration);
            return exchangeRates;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve latests exchange rates for {CacheKey}", cacheKey);
            return [];
        }
    }
}
