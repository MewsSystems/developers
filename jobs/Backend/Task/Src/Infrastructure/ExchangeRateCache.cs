using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Src.Cnb;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRateUpdater.Src.Infrastructure;

public sealed class ExchangeRateCache : IExchangeRateCache
{
    private readonly IDistributedCache _cache;
    private readonly CnbOptions _options;
    private readonly ILogger<ExchangeRateCache> _log;

    public ExchangeRateCache(IDistributedCache cache, IOptions<CnbOptions> options, ILogger<ExchangeRateCache> log)
    {
        _cache = cache;
        _options = options.Value;
        _log = log;
    }

    public async Task<List<ExchangeRate>?> GetAsync(string key, CancellationToken ct)
    {
        try
        {
            string? hit = await _cache.GetStringAsync(key, ct);
            return string.IsNullOrEmpty(hit) ? null : JsonSerializer.Deserialize<List<ExchangeRate>>(hit);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Redis get failed (key: {Key})", key);
            return null;
        }
    }

    public async Task SetAsync(string key, List<ExchangeRate> value, CancellationToken ct)
    {
        try
        {
            string payload = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(
                key,
                payload,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _options.CacheTtl },
                ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Redis set failed (key: {Key})", key);
        }
    }
}
