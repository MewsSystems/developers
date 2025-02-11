using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Caching;

/// <summary>
/// Provides caching functionality using in-memory cache.
/// </summary>
public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;
    private readonly TimeSpan _cacheDuration;
    private readonly ConcurrentDictionary<string, object> _fetchLocks = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheService"/> class.
    /// </summary>
    /// <param name="cache">The memory cache instance.</param>
    /// <param name="logger">The logger instance for logging caching operations.</param>
    /// <param name="cacheSettings">Configuration settings for cache duration.</param>
    public CacheService(IMemoryCache cache, ILogger<CacheService> logger, IOptions<CacheSettings> cacheSettings)
    {
        _cache = cache;
        _logger = logger;
        _cacheDuration = TimeSpan.FromMinutes(cacheSettings.Value.Duration);
    }

    /// <inheritdoc/>
    public async Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> fetchFunction)
    {
        if (_cache.TryGetValue(cacheKey, out T? cachedValue))
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}. Returning cached data.", cacheKey);
            return cachedValue!;
        }

        _logger.LogInformation("Cache miss for key: {CacheKey}. Fetching new data.", cacheKey);

        var fetchTaskSource = new TaskCompletionSource<T?>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Try to insert a new task for this key. If one already exists, wait for its result
        var existingTaskSource = (TaskCompletionSource<T?>)_fetchLocks.GetOrAdd(cacheKey, fetchTaskSource);

        if (existingTaskSource != fetchTaskSource)
        {
            _logger.LogInformation("Waiting for existing fetch task for key: {CacheKey}.", cacheKey);
            return await existingTaskSource.Task;
        }

        try
        {
            var data = await fetchFunction();

            if (data is null)
            {
                _logger.LogWarning("Fetch function returned null for key: {CacheKey}. Data not cached.", cacheKey);
                fetchTaskSource.SetResult(default);
                return default;
            }

            _cache.Set(cacheKey, data, _cacheDuration);
            _logger.LogInformation("Data cached for key: {CacheKey}, Duration: {Duration} minutes.", cacheKey, _cacheDuration.TotalMinutes);

            fetchTaskSource.SetResult(data);
            return data;
        }
        catch (Exception ex)
        {
            fetchTaskSource.SetException(ex);
            throw;
        }
        finally
        {
            _fetchLocks.TryRemove(cacheKey, out _);
        }
    }
}
