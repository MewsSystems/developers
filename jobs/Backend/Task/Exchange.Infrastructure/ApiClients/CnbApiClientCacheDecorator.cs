using Exchange.Application.Abstractions.ApiClients;
using Exchange.Application.Abstractions.Caching;
using Exchange.Infrastructure.DateTimeProviders;

namespace Exchange.Infrastructure.ApiClients;

public class CnbApiClientCacheDecorator(
    ICnbApiClient cnbApiClient,
    ICacheService cacheService,
    ICnbApiClientDataUpdateCalculator cnbApiClientDataUpdateCalculator,
    IDateTimeProvider dateTimeProvider
) : ICnbApiClient
{
    private readonly TimeSpan _shortAbsoluteExpiration = TimeSpan.FromMinutes(10);

    private const string CacheKey = nameof(CnbApiClient);

    public async Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var cached = await cacheService.GetAsync<IEnumerable<CnbExchangeRate>>(CacheKey);

        if (cached is not null)
            return cached;

        var cnbExchangeRates = await cnbApiClient.GetExchangeRatesAsync(cancellationToken);

        await CacheExchangeRatesAsync(cnbExchangeRates);

        return cnbExchangeRates;
    }

    private async Task CacheExchangeRatesAsync(IEnumerable<CnbExchangeRate> cnbExchangeRates)
    {
        var cacheExpiration = CalculateCacheExpiration(cnbExchangeRates);
        await cacheService.SetAsync(CacheKey, cnbExchangeRates, cacheExpiration);
    }

    private TimeSpan CalculateCacheExpiration(IEnumerable<CnbExchangeRate> exchangeRates)
    {
        var lastUpdateDate = GetLastUpdateDate(exchangeRates);
        var expectedUpdateTime = cnbApiClientDataUpdateCalculator.GetNextExpectedUpdateDate(lastUpdateDate);
        var currentTime = dateTimeProvider.Now;

        return expectedUpdateTime > currentTime
            ? expectedUpdateTime - currentTime
            : _shortAbsoluteExpiration;
    }

    private static DateOnly GetLastUpdateDate(IEnumerable<CnbExchangeRate> exchangeRates)
    {
        var firstRate = exchangeRates.First();
        return DateOnly.Parse(firstRate.ValidFor);
    }
}