using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using W4k.Either;

namespace ExchangeRateUpdater;

public sealed class ExchangeRateProvider : IDisposable
{
    private readonly CnbClientCacheProxy _cnbClientCache;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(IOptions<ExchangeRateProviderOptions> options, ICnbClient cnbClient, ILogger<ExchangeRateProvider> logger)
    {
        // 💡 this check is bit silly since we control the creation of provider, let's pretend it's publicly shipped app
        //    (also let's make analyzer happy - public types should check their arguments after all)
        ArgumentNullException.ThrowIfNull(cnbClient);

        _cnbClientCache = new CnbClientCacheProxy(cnbClient, options.Value.CacheTtl);
        _logger = logger;
    }

    public void Dispose()
    {
        _cnbClientCache.Dispose();
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

        return exchangeRatesResult.Match(
            (Currencies: currencies, Logger: _logger),
            static (state, rates) => PickExchangeRates(rates, state.Currencies, state.Logger),
            static (_, _) => new AppError("Failed to fetch exchange rates"));
    }

    private static Either<IReadOnlyCollection<ExchangeRate>, AppError> PickExchangeRates(
        CnbExchangeRatesDto exchangeRates,
        IReadOnlyCollection<Currency> expectedCurrencies,
        ILogger logger) =>
        new ExchangeRateTransformer(logger).GetExchangeRatesForCurrencies(expectedCurrencies, exchangeRates);
}

internal readonly ref struct ExchangeRateTransformer(ILogger logger)
{
    private static readonly Currency DefaultTargetCurrency = new("CZK");

    // 💡 once this gets to hot-path and/or we would want to squeeze every bit of performance, there are few other things we could do...
    //    see benchmarks for more details ( •_•)>⌐■-■ (though, other approaches are rather memory-focused)
    public List<ExchangeRate> GetExchangeRatesForCurrencies(IReadOnlyCollection<Currency> currencies, CnbExchangeRatesDto exchangeRates)
    {
        var ratesByCurrency = exchangeRates.Rates.ToDictionary(r => r.CurrencyCode, StringComparer.Ordinal);

        var rates = new List<ExchangeRate>(currencies.Count);
        foreach (var expectedCurrency in currencies)
        {
            if (ratesByCurrency.TryGetValue(expectedCurrency.Code, out var exchangeRate))
            {
                rates.Add(MapToDomain(exchangeRate));
            }
            else
            {
                logger.LogWarning("Currency '{CurrencyCode}' not found in exchange rates", expectedCurrency.Code);
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
}