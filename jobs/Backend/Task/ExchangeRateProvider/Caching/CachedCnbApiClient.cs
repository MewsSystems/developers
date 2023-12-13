using ExchangeRateUpdater.Model;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Caching
{
    /// <summary>
    /// Caches the retrieved from the Cnb API. 
    /// The strategy is to keep the valid data in the cache until the next expected release. 
    /// And when we still have the outdated data, keep it for some minimal amount of time, before next quering of data source.
    /// </summary>
    public class CachedCnbApiClient : ICnbApiClient
    {
        private readonly ICnbApiClient _client;
        private readonly IAppCache _cache;

        private const string ExchangeRatesKey = nameof(ExchangeRatesKey);
        
        IExchangeRateReleaseDatesProvider _releaseDatesProvider;
        private readonly CachingOptions _config;

        public CachedCnbApiClient(ICnbApiClient client,
            IAppCache cache,
            IExchangeRateReleaseDatesProvider releaseDatesProvider,
            IOptions<CachingOptions> config
            )
        {
            _client = client;
            _cache = cache;
            _releaseDatesProvider = releaseDatesProvider;
            _config = config.Value;
        }

        public async Task<ICollection<ExchangeRate>> GetDailyRates(CancellationToken cancellationToken)
        {
            var rates = await _cache.GetOrAddAsync(ExchangeRatesKey, async (cacheEntry) =>
            {
                var data = await _client.GetDailyRates(cancellationToken);
                if (data is null || data is { Count: 0 })
                {
                    cacheEntry.Dispose();
                    return null!;
                }

                cacheEntry.SetAbsoluteExpiration(CreateExpiration(data));

                return data;
            }).ConfigureAwait(false);

            return rates!;
        }

        private TimeSpan CreateExpiration(ICollection<ExchangeRate> rates)
        {
            var ratesDate = rates.First().Date;
            if (ratesDate < _releaseDatesProvider.GetCurrentReleaseDate()) 
            {
                return _config.OutdatedDataCacheExpiration;
            }

            return _releaseDatesProvider.GetTimeToNextRelease();
        }
    }
}
