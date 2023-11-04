using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Options;
using W4k.Either;

namespace ExchangeRateUpdater;

public sealed class ExchangeRateProvider : IDisposable
{
    private static readonly Currency DefaultTargetCurrency = new("CZK");
    
    private readonly CnbClientCacheProxy _cnbClientCache;

    public ExchangeRateProvider(IOptions<ExchangeRateProviderOptions> options, ICnbClient cnbClient)
    {
        // 💡 this check is bit silly since we control the creation of provider, let's pretend it's publicly shipped app
        //    (also let's make analyzer happy - public types should check their arguments after all)
        ArgumentNullException.ThrowIfNull(cnbClient);

        _cnbClientCache = new CnbClientCacheProxy(cnbClient, options.Value.CacheTtl);
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<Either<IReadOnlyCollection<ExchangeRate>, AppError>> GetExchangeRates(
        IReadOnlyCollection<Currency> currencies,
        CancellationToken cancellationToken)
    {
        var exchangeRatesResult = await _cnbClientCache.GetExchangeRates(cancellationToken);

        return exchangeRatesResult.Match<IReadOnlyCollection<Currency>, Either<IReadOnlyCollection<ExchangeRate>, AppError>>(
            currencies,
            (c, r) => PickRequestedExchangeRates(c, r),
            (_, _) => new AppError { Message = "Failed to fetch exchange rates" });
    }

    private static List<ExchangeRate> PickRequestedExchangeRates(
        IReadOnlyCollection<Currency> currencies,
        CnbExchangeRatesDto exchangeRates)
    {
        var rates = new List<ExchangeRate>(currencies.Count);
        foreach (var rate in exchangeRates.Rates)
        {
            // intentionally iterating over `currencies` over and over - iterating trough short collection is faster than hash lookup
            // -> observe use, optimize accordingly
            if (currencies.Any(c => c.Code == rate.CurrencyCode))
            {
                rates.Add(MapToDomain(rate));
            }
        }

        return rates;
    }
    
    // 💡 mapping could be moved to separate (static) class, for simplicity I kept it here as this is only use so far
    //   (definitely no space for AutoMapper *wink*)
    private static ExchangeRate MapToDomain(CnbExchangeRate rate) =>
        new(
            sourceCurrency: new Currency(rate.CurrencyCode),
            targetCurrency: DefaultTargetCurrency,
            value: rate.ExchangeRate / rate.Amount);

    public void Dispose()
    {
        _cnbClientCache.Dispose();
    }
}