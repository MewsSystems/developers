using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    // We could use a basic dictionary for caching, but we will use a distributed cache
    // to allow for better scalability and to avoid issues with multiple instances of the application.
    // We assumed 5 minutes of sliding expiration, but this could be changed to a more suitable value.
    internal sealed class CachedExchangeRateFetcher(
        IDistributedCache cache, 
        IExchangeRateFetcher underlying) : IExchangeRateFetcher
    {
        private readonly IDistributedCache cache = cache;
        private readonly IExchangeRateFetcher underlying = underlying;

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken cancellationToken = default)
        {
            const string cacheKey = "exchange-rates";
            var cached = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(cached)!;
            }

            var rates = await underlying.GetExchangeRates(cancellationToken);
            var serialized = JsonSerializer.Serialize(rates);

            await cache.SetStringAsync(
                cacheKey, 
                serialized, 
                new DistributedCacheEntryOptions
                {                    
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                }, 
                cancellationToken);

            return rates;
        }
    }
}
