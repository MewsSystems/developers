using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRates.Infrastructure.Repositories;

internal class CacheExchangeRateRepository : IExchangeRateRepository
{
    private const string _defaultKey = "NOW";

    private readonly IExchangeRateRepository _repository;

    private readonly IDistributedCache _distributedCache;

    private readonly IOptions<CacheSettings> _options;

    public CacheExchangeRateRepository(IOptions<CacheSettings> options, IDistributedCache distributedCache, IExchangeRateRepository repository)
    {
        _options = options;
        _distributedCache = distributedCache;
        _repository = repository;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? day, CancellationToken cancellationToken = default)
    {
        var rates = await GetCachedRatesAsync(day, cancellationToken);
        if (rates == null)
        {
            rates = await _repository.GetExchangeRatesAsync(day, cancellationToken);

            await SetCachedRatesAsync(day, rates, cancellationToken);
        }

        return rates;
    }

    private async Task<IEnumerable<ExchangeRate>> GetCachedRatesAsync(DateTime? day, CancellationToken cancellationToken)
    {
        var value = await _distributedCache.GetStringAsync(GetKey(day), cancellationToken);
        if (value != null)
        {
            return JsonSerializer.Deserialize<List<ExchangeRate>>(value) ??
                throw new InvalidDataException("Invalid cached exchange rates format.");
        }

        return null;
    }

    private async Task SetCachedRatesAsync(DateTime? day, IEnumerable<ExchangeRate> rates, CancellationToken cancellationToken)
    {
        var jsonString = JsonSerializer.Serialize(rates);

        await _distributedCache.SetStringAsync(GetKey(day), jsonString, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.Value.ExpirationInHours),
        }, cancellationToken);
    }

    private static string GetKey(DateTime? day) => day?.ToString() ?? _defaultKey;
}
