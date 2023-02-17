using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
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
        return GetExchangeRatesUnion(currencies);
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
        return GetExchangeRatesUnion(currencies);
    }

    private IEnumerable<ExchangeRate> GetExchangeRatesUnion(IEnumerable<Currency> currencies)
    {
        IEnumerable<ExchangeRate> result = new List<ExchangeRate>();
        foreach (var item in currencies)
        {
            var sourceRates = _exchangeRateSource.GetSourceExchangeRates(item);
            var targetRates = _exchangeRateSource.GetTargetExchangeRates(item);
            var allRates = sourceRates.Union(targetRates);
            result = result.Union(allRates);
        }

        return result;
    }
}
