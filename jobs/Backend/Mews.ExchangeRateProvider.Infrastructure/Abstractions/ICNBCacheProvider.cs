using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Microsoft.Extensions.Caching.Memory;

namespace Mews.ExchangeRateProvider.Infrastructure.Abstractions
{
    public interface ICNBCacheProvider
    {
        IEnumerable<ExchangeRate>? GetFromCache(string key);
        void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class;
        void ClearCache(string key);
    }
}
