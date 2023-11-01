using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;
using Mews.ExchangeRateUpdater.Infrastructure.HttpClients;
using Microsoft.Extensions.Caching.Memory;

namespace Mews.ExchangeRateUpdater.Infrastructure.Data.Repositories;

/// <summary>
/// Exchange rates repository implementation.
/// </summary>
public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IMemoryCache _cache;
    private readonly ICzechNationalBankApiClient _czechNationalBankApiClient;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="czechNationalBankApiClient"></param>
    /// <param name="cache"></param>
    public ExchangeRateRepository(ICzechNationalBankApiClient czechNationalBankApiClient, IMemoryCache cache)
    {
        _cache = cache;
        _czechNationalBankApiClient = czechNationalBankApiClient;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetTodayExchangeRatesAsync()
    {
        var bankRates = await _czechNationalBankApiClient.GetTodayExchangeRatesAsync();

        return bankRates.Rates.Select(rate => new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"),
            decimal.Divide(rate.Rate, rate.Amount)));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetCachedTodayExchangeRatesAsync()
    {
        const string cacheKeyPrefix = "TodayExchangeRates-";
        var currentDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");
        var cacheKey = cacheKeyPrefix + currentDate;

        if (_cache.TryGetValue<IEnumerable<ExchangeRate>>(cacheKey, out var cacheEntry) && cacheEntry is not null)
        {
            return cacheEntry;
        }

        var exchangeRates = (await GetTodayExchangeRatesAsync()).ToList();

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };

        if (!exchangeRates.Any()) return exchangeRates;

        _cache.Set(cacheKey, exchangeRates, cacheEntryOptions);

        return exchangeRates;
    }
}