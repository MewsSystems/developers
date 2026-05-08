using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Core.Providers;

public class CzechNationalBankExchangeRateProvider(
        IExchangeRateVendor exchangeRateVendor,
        IMemoryCache? cache = null
    ) : IExchangeRateProvider
{
    private const string BaseCurrencyCode = "CZK";
    private const string CacheKey = $"ExchangeRates:{BaseCurrencyCode}";
    private readonly IMemoryCache _cache = cache ?? new MemoryCache(new MemoryCacheOptions());

    public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        currencies = currencies.ToArray();
        if (!currencies.Any()) return [];

        var allRates = await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return await exchangeRateVendor.GetExchangeRates(BaseCurrencyCode);
        }) ?? [];

        return allRates
                .Where(rate => currencies.Any(currency => currency.ToString() == rate.TargetCurrency.ToString()))
                .ToList();
    }
}