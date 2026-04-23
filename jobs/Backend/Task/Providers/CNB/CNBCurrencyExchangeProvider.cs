using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers.CNB;
public class CNBCurrencyExchangeProvider : IExchangeRateProvider
{
    private readonly ICNBExchangeRateApiClient _client;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;
    private readonly string _cacheKeyPrefix;

    public CNBCurrencyExchangeProvider(
        ICNBExchangeRateApiClient client,
        IMemoryCache cache,
        IOptions<CNBExchangeRateProviderOptions> options)
    {
        _client = client;
        _cache = cache;
        _cacheKeyPrefix = nameof(CNBCurrencyExchangeProvider);
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = options.Value.CacheAbsoluteExpiration,
            SlidingExpiration = options.Value.CacheSlidingExpiration
        };
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var targetDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var cacheKey = $"{_cacheKeyPrefix}_{targetDate:yyyy-MM-dd}";

        if (!_cache.TryGetValue<List<ExchangeRate>>(cacheKey, out var cachedRates))
        {
            cachedRates = (await _client.FetchExchangeRatesAsync(targetDate)).ToList();

            _cache.Set(cacheKey, cachedRates, _cacheOptions);
        }

        var sourceCurrencies = currencies.ToHashSet();

        return cachedRates.Where(x => sourceCurrencies.Contains(x.SourceCurrency));
    }
}