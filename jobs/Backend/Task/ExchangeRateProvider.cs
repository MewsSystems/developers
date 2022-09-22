using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

// https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/

public class ExchangeRateProvider
{
    private static readonly IDictionary<DateOnly, IEnumerable<ExchangeRate>> CACHE =
        new ConcurrentDictionary<DateOnly, IEnumerable<ExchangeRate>>();
    
    private readonly IExchangeRateLoader _loader;

    private readonly IExchangeRateParser _parser;
    
    private readonly IDateProvider _dateProvider;

    public ExchangeRateProvider(
        IExchangeRateLoader loader, 
        IExchangeRateParser parser,
        IDateProvider dateProvider)
    {
        _loader = loader;
        _parser = parser;
        _dateProvider = dateProvider;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        var enumerated = currencies.ToList();
        if (!enumerated.Any())
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var today = _dateProvider.ForToday();
        if (!CACHE.ContainsKey(today))
        {
            var stream = await _loader.ReadAsync().ConfigureAwait(false);
            var rates = await _parser.ParseAsync(stream).ConfigureAwait(false);
            
            CACHE.Add(today, rates);
        }

        return CACHE[today].Where(w => enumerated.Any(a => a.Code == w.TargetCurrency.Code));
    }
}