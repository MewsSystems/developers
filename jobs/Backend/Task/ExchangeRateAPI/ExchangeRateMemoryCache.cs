using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.ExchangeRateAPI
{
    public class ExchangeRateMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 1024
            });
    }
}
