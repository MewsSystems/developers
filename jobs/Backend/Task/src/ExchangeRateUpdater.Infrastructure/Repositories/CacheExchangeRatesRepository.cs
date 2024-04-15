using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a repository for caching exchange rates retrieved from another exchange rates repository.
    /// </summary>
    internal class CacheExchangeRatesRepository : IExchangeRatesRepository
    {
        private readonly IExchangeRatesRepository _repository;
        private readonly IMemoryCache _memoryCache;

        private const string CacheKeyPrefix = "ExchangeRates";
        private const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheExchangeRatesRepository"/> class.
        /// </summary>
        /// <param name="repository">The underlying exchange rates repository to cache data from.</param>
        /// <param name="memoryCache">The memory cache instance for caching exchange rates.</param>
        public CacheExchangeRatesRepository([FromKeyedServices(nameof(ExchangeRatesRepository))] IExchangeRatesRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date)
        {
            var cacheKey = $"{CacheKeyPrefix}-{date?.ToString(DateFormat)}";

            if (!_memoryCache.TryGetValue<IEnumerable<ExchangeRate>>(cacheKey, out var cacheEntry))
            {
                var cacheValue = (await _repository.GetExchangeRatesAsync(date));

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(cacheKey, cacheValue, cacheEntryOptions);
                return cacheValue;
            }

            return cacheEntry ?? [];
        }
    }
}
