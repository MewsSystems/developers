using ExchangeRates.Domain;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace ExchangeRates.App.Caching
{
    /// <summary>
    /// The Redis implementation of the caching service.
    /// </summary>
    internal class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisOptions _options;
        public RedisCacheService(IDistributedCache cache, IOptions<RedisOptions> options)
        {
            _cache = cache;
            _options = options.Value;
        }

        public T GetCachedData<T>(string key)
        {
            var jsonData = _cache.GetString(key);

            return jsonData == null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }

        //the key is in format "usd/czk"
        private void SetCachedData<T>(string key, T data, TimeSpan cacheDuration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };

            var jsonData = JsonSerializer.Serialize(data);
            _cache.SetString(key, jsonData, options);
        }

        public void SetCachedData(string key, ExchangeRate value)
        {
            SetCachedData(key, value, TimeSpan.FromSeconds(CalculateExpiry()));
        }

        //the cache will automatically expire on the given time "hh:mm"
        private double CalculateExpiry()
        {
            var hours = _options.ExpireAt.Hours;
            var minutes = _options.ExpireAt.Minutes;
            var now = DateTime.Now;
            var expireAt = now.Date.AddHours(hours).AddMinutes(minutes); //expire today at hh:mm
            if (now.Hour > hours || (now.Hour == hours && now.Minute > minutes))
            {
                expireAt = now.Date.AddDays(1).AddHours(hours).AddMinutes(minutes); //expire tomorrow at hh:mm
            }
            var expiry = (expireAt - now).TotalSeconds;
            return expiry;
        }
    }

    public class RedisOptions
    {
        public TimeSpan ExpireAt { get; set; }
    }
}
