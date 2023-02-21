using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateSource _exchangeRateSource;

    public ExchangeRateProvider(IExchangeRateSource exchangeRateSource)
    {
        _exchangeRateSource = exchangeRateSource;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        await _exchangeRateSource.LoadAsync();
        return GetExchangeRatesInternal(currencies);
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        _exchangeRateSource.LoadAsync().Wait();
        return GetExchangeRatesInternal(currencies);
    }

    private IEnumerable<ExchangeRate> GetExchangeRatesInternal(IEnumerable<Currency> currencies)
    {
        var exchangeRates = _exchangeRateSource.GetExchangeRates();
        return FilterExchangeRates(exchangeRates, currencies);
    }

    private static IEnumerable<ExchangeRate> FilterExchangeRates(IEnumerable<ExchangeRate> exchangeRates, IEnumerable<Currency> currencies)
    {
        var hashSet = new HashSet<Currency>(currencies);
        return exchangeRates.Where(er => currencies.Contains(er.SourceCurrency) || currencies.Contains(er.TargetCurrency));
    }
}
