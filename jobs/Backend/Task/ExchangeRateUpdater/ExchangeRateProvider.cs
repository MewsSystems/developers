using System.Runtime.InteropServices;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using W4k.Either;

namespace ExchangeRateUpdater;

// 💡 considering `ExchangeRateProvider` as "CNB" specific component and main unit - that's also reason for coupling with other components
public sealed class ExchangeRateProvider : IDisposable
{
    private readonly CnbClientCacheProxy _cnbClientCache;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(IOptions<ExchangeRateProviderOptions> options, ICnbClient cnbClient, ILogger<ExchangeRateProvider> logger)
    {
        // 💡 this check is bit silly since we control the creation of provider, but public type is public type ¯\_(ツ)_/¯
        ArgumentNullException.ThrowIfNull(cnbClient);
        ArgumentNullException.ThrowIfNull(logger);

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
        var exchangeRatesResult = await _cnbClientCache.GetExchangeRates(cancellationToken).ConfigureAwait(false);

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

    // 💡 there are other algorithms to solve this, though it looks like there's no universal one (covering various lengths of input)
    //    see benchmarks for other (more readable) algorithms ( •_•)>⌐■-■
    public List<ExchangeRate> GetExchangeRatesForCurrencies(IReadOnlyCollection<Currency> currencies, CnbExchangeRatesDto exchangeRates)
    {
        var currenciesSpan = currencies switch
        {
            Currency[] a => a.AsSpan(),
            List<Currency> l => CollectionsMarshal.AsSpan(l),
            _ => CollectionsMarshal.AsSpan(currencies.ToList()),
        };

        var exchangeRatesSpan = exchangeRates.Rates switch
        {
            List<CnbExchangeRate> l => CollectionsMarshal.AsSpan(l),
            _ => CollectionsMarshal.AsSpan(exchangeRates.Rates.ToList()),
        };

        // Following algorithm iterates trough requested currencies, moves pointer within existing exchange rates,
        // and once pointers meet on same value, exchange rate is added to result list.
        //
        // Algorithm requires both collections to be sorted first.
        //
        // Complexity:
        // - sorting: O(m log m + n log n)
        // - iteration trough currencies: O(m)
        // - iteration trough exchange rates: O(n)
        // -> total complexity: O(m log m + n log n)
        //
        // currencies:
        // [ EUR, GBP, USD ]
        //    ^- `currencyIdx`
        //
        // exchange rates:
        // [ CZK, EUR, GBP, HUF, TOP, USD ]
        //    ^- smaller than EUR, move pointer
        //         ^- `rateIdx`
        //
        // explanation:
        // 1. move `rateIdx` while it's smaller than `currencyIdx` (meaning pointing value)
        // 2. if we haven't reached end of `exchangeRates` yet...
        // 3. ... and `rateIdx` points to same currency as under `currencyIdx`, add to result
        currenciesSpan.Sort(CurrencyComparer.Instance);
        exchangeRatesSpan.Sort(ExchangeRateCurrencyComparer.Instance);

        var exchangeRatesLength = exchangeRatesSpan.Length;
        var rates = new List<ExchangeRate>(currenciesSpan.Length);

        int rateIdx = 0;
        for (int currencyIdx = 0; currencyIdx < currenciesSpan.Length; currencyIdx++)
        {
            var currency = currenciesSpan[currencyIdx];

            // move `rateIdx` pointer while pointing value (exchange rate code) is smaller than currency code,
            // e.g. `CZK` < `EUR` --> move pointer
            while (rateIdx < exchangeRatesLength
                   && string.CompareOrdinal(exchangeRatesSpan[rateIdx].CurrencyCode, currency.Code) < 0)
            {
                ++rateIdx;
            }

            // compare pointed values, if they are same, add to result
            if (rateIdx < exchangeRatesLength
                && currency.Code == exchangeRatesSpan[rateIdx].CurrencyCode)
            {
                rates.Add(MapToDomain(exchangeRatesSpan[rateIdx]));
            }
            else
            {
                logger.LogWarning("Currency '{CurrencyCode}' not found in exchange rates", currency);
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

    private sealed class CurrencyComparer : IComparer<Currency>
    {
        public static readonly CurrencyComparer Instance = new();
        public int Compare(Currency? x, Currency? y) => string.CompareOrdinal(x!.Code, y!.Code);
    }

    private sealed class ExchangeRateCurrencyComparer : IComparer<CnbExchangeRate>
    {
        public static readonly ExchangeRateCurrencyComparer Instance = new();
        public int Compare(CnbExchangeRate? x, CnbExchangeRate? y) => string.CompareOrdinal(x!.CurrencyCode, y!.CurrencyCode);
    }
}