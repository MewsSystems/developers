using System.Text.Json;
using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure
{
    /// <summary>
    /// Distributed caching provider with CNB-aware cache durations.
    /// </summary>
    public sealed class DistributedCachingExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProvider _innerProvider;
        private readonly IDistributedCache _distributedCache;
        private readonly CnbCacheStrategy _cacheStrategy;
        private readonly ILogger<DistributedCachingExchangeRateProvider> _logger;

        private const string CacheKey = "cnb_all_rates";

        /// <summary>
        /// Gets the name of this provider (delegates to inner provider).
        /// </summary>
        public string Name => _innerProvider.Name;

        /// <summary>
        /// Gets the priority of this provider (delegates to inner provider).
        /// </summary>
        public int Priority => _innerProvider.Priority;

        /// <summary>
        /// Determines whether this provider can handle the specified currencies (delegates to inner provider).
        /// </summary>
        public bool CanHandle(IEnumerable<Currency> currencies) => _innerProvider.CanHandle(currencies);

        public DistributedCachingExchangeRateProvider(
            IExchangeRateProvider innerProvider,
            IDistributedCache distributedCache,
            CnbCacheStrategy cacheStrategy,
            ILogger<DistributedCachingExchangeRateProvider> logger)
        {
            _innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _cacheStrategy = cacheStrategy ?? throw new ArgumentNullException(nameof(cacheStrategy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies,
            CancellationToken cancellationToken = default)
        {
            var requestedCurrencies = currencies?.ToList() ?? [];
            if (!requestedCurrencies.Any())
            {
                return [];
            }

            // Try to get all rates from cache
            var cachedRates = await GetAllRatesFromCacheAsync(cancellationToken);
            if (cachedRates != null)
            {
                var missingCurrencies = requestedCurrencies.Where(c => cachedRates.All(r => r.SourceCurrency.Code != c.Code)).ToList();

                if (missingCurrencies.Count != 0)
                {
                    _logger.LogDebug("Cache partial hit - fetching {Count} missing rates", missingCurrencies.Count);

                    _logger.LogDebug("Cache miss - fetching fresh rates");
                    var fetchedRates = await _innerProvider.GetExchangeRatesAsync(missingCurrencies, cancellationToken);

                    // Cache all rates with CNB-aware duration
                    cachedRates = cachedRates.Concat(fetchedRates).ToList();
                    await CacheAllRatesAsync(cachedRates, cancellationToken);

                    return cachedRates;
                }
                
                _logger.LogDebug("Cache hit - returning {Count} rates from cache", cachedRates.Count);
                return cachedRates;
            }

            // Cache miss or partial hit - fetch all fresh data
            _logger.LogDebug("Cache miss - fetching fresh rates");
            var freshRates = await _innerProvider.GetExchangeRatesAsync(requestedCurrencies, cancellationToken);
            var freshRatesList = freshRates.ToList();

            // Cache all rates with CNB-aware duration
            await CacheAllRatesAsync(freshRatesList, cancellationToken);

            return freshRatesList;
        }

        private async Task<List<ExchangeRate>?> GetAllRatesFromCacheAsync(CancellationToken cancellationToken)
        {
            try
            {
                var cachedBytes = await _distributedCache.GetAsync(CacheKey, cancellationToken);
                if (cachedBytes?.Length > 0)
                {
                    try
                    {
                        return JsonSerializer.Deserialize<List<ExchangeRate>>(cachedBytes);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to deserialize cached rates");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read from cache");
            }

            return null;
        }

        private async Task CacheAllRatesAsync(List<ExchangeRate> rates, CancellationToken cancellationToken)
        {
            try
            {
                // Use CNB strategy for smart cache duration
                var cacheOptions = _cacheStrategy.GetCacheOptions();
                
                var serialized = JsonSerializer.SerializeToUtf8Bytes(rates);
                var distributedOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheOptions.Duration
                };

                await _distributedCache.SetAsync(CacheKey, serialized, distributedOptions, cancellationToken);
                
                _logger.LogInformation(
                    "Cached {Count} exchange rates for {Duration} (CNB-aware)", 
                    rates.Count, cacheOptions.Duration);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to cache exchange rates");
            }
        }
    }
}