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
    public async Task<Either<IReadOnlyCollection<ExchangeRate>, Error>> GetExchangeRates(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken)
    {
        var exchangeRatesResult = await _cnbClientCache.GetExchangeRates(cancellationToken);

        return exchangeRatesResult.Match<IEnumerable<Currency>, Either<IReadOnlyCollection<ExchangeRate>, Error>>(
            currencies,
            (wantedCurrencies, exchangeRatesPayload) =>
            {
                return exchangeRatesPayload.Rates
                    .Where(r => wantedCurrencies.Any(c => c.Code == r.CurrencyCode))
                    .Select(
                        r => new ExchangeRate(
                            sourceCurrency: new Currency(r.CurrencyCode),
                            targetCurrency: new Currency("CZK"),
                            value: r.ExchangeRate / r.Amount))
                    .ToList();
            },
            (_, _) => new Error { Message = "Failed to fetch exchange rates" });
    }

    public void Dispose()
    {
        _cnbClientCache.Dispose();
    }
}