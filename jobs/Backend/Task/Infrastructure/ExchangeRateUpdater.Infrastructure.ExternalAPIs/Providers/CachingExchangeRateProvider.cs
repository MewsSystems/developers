using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Options;
using ExchangeRateUpdater.Infrastructure.Interface.ExternalAPIs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Providers;

/// <inheritdoc />
internal class CachingExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateClient _exchangeRateClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ExchangeRateProviderCachingOptions _options;
    private readonly TimeProvider _timeProvider;

    public CachingExchangeRateProvider(IExchangeRateClient exchangeRateClient, IMemoryCache memoryCache, IOptions<ExchangeRateProviderCachingOptions> options, TimeProvider timeProvider)
    {
        _exchangeRateClient = exchangeRateClient;
        _memoryCache = memoryCache;
        _timeProvider = timeProvider;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var allRates = await GetAllRates(cancellationToken: cancellationToken);
        return allRates.FilterAndConvertRates(currencies);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(DateTime date, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        var allRates = await GetAllRates(date, cancellationToken);
        return allRates.FilterAndConvertRates(currencies);
    }

    /// <summary>
    /// Get all rates for a given date from either the cache or the client.
    /// </summary>
    /// <remarks>
    /// Uses ValueTask rather than Task to avoid the overhead for the cache path that does NOT do any async.
    /// </remarks>
    /// <returns>All rates</returns>
    private async ValueTask<IEnumerable<CnbRate>?> GetAllRates(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        var key = GetCacheKey(date);
        if (_memoryCache.TryGetValue(key, out IEnumerable<CnbRate>? allRates)) return allRates;

        return await FetchAllRates(date, cancellationToken);
    }

    /// <summary>
    /// Fetch rates from the client and set them in the cache.
    /// </summary>
    /// <param name="date">Date to perform fetch for.</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>All Rates</returns>
    private async Task<IEnumerable<CnbRate>?> FetchAllRates(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<CnbRate>? allRates;
        if (date.HasValue)
        {
            allRates = await _exchangeRateClient.GetRates(date.Value, cancellationToken);
        }
        else
        {
            allRates = await _exchangeRateClient.GetRates(cancellationToken);
        }
        
        if (allRates is null) return Enumerable.Empty<CnbRate>();
           
        allRates = allRates.ToArray();
        
        var cacheKey = GetCacheKey(allRates.FirstOrDefault()?.ValidFor ?? date);
        _memoryCache.Set(cacheKey, allRates, GetCacheEntryOptions());
        return allRates;
    }

    private MemoryCacheEntryOptions GetCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
        {
            SlidingExpiration = _options.SlidingCacheDuration,
            AbsoluteExpirationRelativeToNow = _options.MaxCacheDuration,
        };
    }
    
    private string GetCacheKey(DateTime? date)
    {
        return date.HasValue 
            ? $"exchange-rates-{date:yyyy-MM-dd}"
            : $"exchange-rates-{_timeProvider.GetUtcNow():yyyy-MM-dd}";
    }
}