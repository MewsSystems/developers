using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Shared.Caching;

public class RedisCacheService(IDistributedCache cache, IOptions<RedisSettings> settings) : ICache
{
    private readonly IDistributedCache _cache = cache;
    private readonly RedisSettings _settings = settings.Value;

    public T GetData<T>(string key)
    {
        var json = _cache.GetString(key);

        return json != null ? JsonSerializer.Deserialize<T>(json) : default;
    }

    public void SetData<T>(string key, T value)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _settings.ExpirationThreshold
        };

        var jsonData = JsonSerializer.Serialize(value);
        _cache.SetString(key, jsonData, options);
    }
}
