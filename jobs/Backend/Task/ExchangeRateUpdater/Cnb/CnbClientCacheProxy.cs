using System.Diagnostics.CodeAnalysis;
using W4k.Either;

namespace ExchangeRateUpdater.Cnb;

// 💡 intentionally not using built-in `MemoryCache` or any other caching library, this should be good enough & w/o external dependencies
internal sealed class CnbClientCacheProxy(ICnbClient cnbClient, TimeSpan ttl) : IDisposable
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    private CnbExchangeRatesDto? _cachedExchangeRates;
    private DateTime _cachedExchangeRatesTimestamp = DateTime.MinValue;

    public void Dispose()
    {
        _lock.Dispose();
    }

    public async ValueTask<Either<CnbExchangeRatesDto, CnbError>> GetExchangeRates(CancellationToken cancellationToken)
    {
        if (IsCacheHit(out var cachedExchangeRates))
        {
            return cachedExchangeRates;
        }

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (IsCacheHit(out cachedExchangeRates))
            {
                return cachedExchangeRates;
            }

            var result = await cnbClient.GetCurrentExchangeRates(cancellationToken).ConfigureAwait(false);

            return result.Match<Either<CnbExchangeRatesDto, CnbError>>(
                exchangeRates =>
                {
                    _cachedExchangeRates = exchangeRates;
                    _cachedExchangeRatesTimestamp = DateTime.UtcNow;
                    return exchangeRates;
                },
                error => error);
        }
        finally
        {
            _lock.Release();
        }

        bool IsCacheHit([NotNullWhen(true)] out CnbExchangeRatesDto? exchangeRates)
        {
            exchangeRates = _cachedExchangeRates;
            return exchangeRates is not null
                && _cachedExchangeRatesTimestamp.Add(ttl) >= DateTime.UtcNow;
        }
    }
}