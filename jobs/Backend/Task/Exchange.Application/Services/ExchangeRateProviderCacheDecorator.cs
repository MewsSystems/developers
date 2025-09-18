using Exchange.Application.Abstractions.Caching;
using Exchange.Domain.Entities;
using Exchange.Domain.ValueObjects;

namespace Exchange.Application.Services;

public class ExchangeRateProviderCacheDecorator(
    IExchangeRateProvider exchangeRateProvider,
    ICacheService cacheService
)
    : IExchangeRateProvider
{
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default
    )
    {
        var cacheKey = GetCacheKey(currencies);

        var cached = await cacheService.GetAsync<IEnumerable<ExchangeRate>>(cacheKey);

        if (cached is not null)
            return cached;

        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies, cancellationToken);

        await cacheService.SetAsync(cacheKey, rates);

        return rates;
    }

    private static string GetCacheKey(IEnumerable<Currency> currencies)
    {
        const string prefix = nameof(ExchangeRateProvider);
        return prefix + ":" + string.Join("-", currencies.OrderBy(c => c.Code).Select(c => c.Code));
    }
}