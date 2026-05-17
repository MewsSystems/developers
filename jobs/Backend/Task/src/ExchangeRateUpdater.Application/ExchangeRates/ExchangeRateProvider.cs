using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.Clients;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.ExchangeRates;
using Microsoft.Extensions.Caching.Memory;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICzbExchangeRateClient _czbExchangeRateClient;
    private readonly IMemoryCache _memoryCache;

    public ExchangeRateProvider(ICzbExchangeRateClient czbExchangeRateClient, IMemoryCache memoryCache)
    {
        _czbExchangeRateClient = czbExchangeRateClient;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var cacheKey = "ExchangeRates";
        if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> cachedRates))
        {
            List<ExchangeRate> rates = new();

            foreach (Currency currency in currencies)
            {
                var exchangeRate = await _czbExchangeRateClient.GetExchangeRate(currency.Code);

                if (exchangeRate is not null && exchangeRate.IsSuccess)
                {
                    rates.AddRange(exchangeRate.Value);
                }
            }

            _memoryCache.Set(cacheKey, rates, TimeSpan.FromHours(24));

            return Result<IEnumerable<ExchangeRate>>.Success(rates);
        }

        return Result<IEnumerable<ExchangeRate>>.Success(cachedRates);
    }
}
