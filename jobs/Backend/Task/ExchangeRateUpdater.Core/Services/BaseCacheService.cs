using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Services
{
    public class BaseCacheService
    {
        public string CacheKey { get; }
        public DistributedCacheEntryOptions CacheOptions { get; }

        public BaseCacheService(string cacheKey, int absoluteExpirationMinutes, int slidingExpirationMinutes)
        {
            CacheKey = cacheKey;
            CacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes)
            };
        }

        public static bool IsCacheEmpty(byte[]? data)
        {
            return data == null || data.Length == 0;
        }

        public static byte[] ConvertListToByteArray<T>(List<T> list)
        {
            string jsonString = JsonSerializer.Serialize(list);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public static List<T> ConvertByteArrayToList<T>(byte[] byteArray)
        {
            string jsonString = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}
