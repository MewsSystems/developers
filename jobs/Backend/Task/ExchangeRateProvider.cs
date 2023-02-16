using System.Collections.Generic;
using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater;

public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateSource _exchangeRateSource;

    public ExchangeRateProvider(IExchangeRateSource exchangeRateSource)
    {
        _exchangeRateSource = exchangeRateSource;
    }

    public async IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        await _exchangeRateSource.LoadAsync();
        foreach (var item in currencies)
        {
            if (_exchangeRateSource.ExchangeRates.TryGetValue((item.Code, "CZK"), out var exchangeRate))
            {
                yield return exchangeRate;
            }
        }
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
        foreach (var item in currencies)
        {
            if (_exchangeRateSource.ExchangeRates.TryGetValue((item.Code, "CZK"), out var exchangeRate))
            {
                yield return exchangeRate;
            }
        }
    }
}
