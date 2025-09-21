using Exchange.Application.Abstractions.ApiClients;
using Exchange.Application.Abstractions.Caching;
using Exchange.Infrastructure.DateTimeProviders;
using Microsoft.Extensions.Logging;

namespace Exchange.Infrastructure.ApiClients;

public class CnbApiClientCacheDecorator(
    ICnbApiClient cnbApiClient,
    ICacheService cacheService,
    ICnbApiClientDataUpdateCalculator cnbApiClientDataUpdateCalculator,
    IDateTimeProvider dateTimeProvider,
    ILogger<CnbApiClientCacheDecorator> logger
) : ICnbApiClient
{
    private readonly TimeSpan _shortAbsoluteExpiration = TimeSpan.FromMinutes(10);

    private const string CacheKey = nameof(CnbApiClient);

    public async Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        var cached = await cacheService.GetAsync<IEnumerable<CnbExchangeRate>>(CacheKey);

        if (cached is not null)
        {
            logger.LogInformation("Data retrieved from cache.");
            return cached;
        }

        var cnbExchangeRates = await cnbApiClient.GetExchangeRatesAsync(cancellationToken);

        await CacheExchangeRatesAsync(cnbExchangeRates);

        return cnbExchangeRates;
    }

    private async Task CacheExchangeRatesAsync(IEnumerable<CnbExchangeRate> cnbExchangeRates)
    {
        logger.LogInformation("Caching exchange rates.");
        var cacheExpiration = CalculateCacheExpiration(cnbExchangeRates);
        logger.LogInformation(
            "Cache expiration: {Days} days, {Hours} hours, {Minutes} minutes, {Seconds} seconds",
            cacheExpiration.Days,
            cacheExpiration.Hours,
            cacheExpiration.Minutes,
            cacheExpiration.Seconds
        );
        await cacheService.SetAsync(CacheKey, cnbExchangeRates, cacheExpiration);
    }

    private TimeSpan CalculateCacheExpiration(IEnumerable<CnbExchangeRate> exchangeRates)
    {
        var lastUpdateDate = GetLastUpdateDate(exchangeRates);
        var expectedUpdateTime = cnbApiClientDataUpdateCalculator.GetNextExpectedUpdateDate(lastUpdateDate);
        logger.LogInformation("Expected update time: {ExpectedUpdateTime}", expectedUpdateTime);
        var currentTime = dateTimeProvider.Now;

        return expectedUpdateTime > currentTime
            ? expectedUpdateTime - currentTime
            : _shortAbsoluteExpiration;
    }

    private DateOnly GetLastUpdateDate(IEnumerable<CnbExchangeRate> exchangeRates)
    {
        var firstRate = exchangeRates.First();
        var lastUpdateDate = DateOnly.Parse(firstRate.ValidFor);
        logger.LogInformation("Last update date: {LastUpdateDate}", lastUpdateDate);
        return lastUpdateDate;
    }
}