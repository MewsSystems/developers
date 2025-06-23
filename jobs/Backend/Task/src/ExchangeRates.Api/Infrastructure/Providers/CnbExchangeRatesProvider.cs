using ExchangeRates.Api.Infrastructure.Clients.Cnb;
using Microsoft.Extensions.Caching.Distributed;

namespace ExchangeRates.Api.Infrastructure.Providers;

public class CnbExchangeRatesProvider : IExchangeRatesProvider
{
    private static readonly TimeSpan _expirationTime = new(12, 31, 00);
    private const string _exchangeRateCacheKey = "exchange-rates";

    private readonly ICnbHttpClient _cnbHttpClient;
    private readonly IDistributedCache _distributedCache;

    public CnbExchangeRatesProvider(ICnbHttpClient cnbHttpClient,
        IDistributedCache distributedCache)
    {
        _cnbHttpClient = cnbHttpClient;
        _distributedCache = distributedCache;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRatesAsync()
    {
        var exchangeRatesContent = await _distributedCache.GetStringAsync(_exchangeRateCacheKey);

        if (string.IsNullOrEmpty(exchangeRatesContent))
        {
            var exchangeRatesResult = await _cnbHttpClient.GetExchangeRatesAsync(DateTime.UtcNow);

            if (!exchangeRatesResult.IsSuccess)
                return exchangeRatesResult;

            await _distributedCache.SetStringAsync(_exchangeRateCacheKey, JsonSerializer.Serialize(exchangeRatesResult.Value), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = GetCacheExpiration()
            });

            return exchangeRatesResult;
        }

        var exchangeRates = JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(exchangeRatesContent!);

        var result = Result<IEnumerable<ExchangeRate>>.Success(exchangeRates!);

        return result;
    }

    private static DateTime GetCacheExpiration()
    {
        var expiration = new DateTime(DateTime.UtcNow.Year,
                                      DateTime.UtcNow.Month,
                                      DateTime.UtcNow.Day,
                                      _expirationTime.Hours,
                                      _expirationTime.Minutes,
                                      _expirationTime.Seconds);

        if (DateTime.UtcNow.TimeOfDay < _expirationTime)
        {
            return expiration;
        }

        return expiration.AddDays(1);
    }
}
