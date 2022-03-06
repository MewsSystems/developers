using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Providers.CNB;

public class CNBExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICNBExchangeRateService _cnbExchangeRateService;

    public CNBExchangeRateProvider(ICNBExchangeRateService cnbExchangeRateService)
    {
        _cnbExchangeRateService = cnbExchangeRateService;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        if (currencies == null) throw new ArgumentNullException(nameof(currencies));

        var allRates = await _cnbExchangeRateService.GetExchangeRatesAsync();
        return FilterExchangeRates(allRates, currencies);
    }

    private static IEnumerable<ExchangeRate> FilterExchangeRates(IEnumerable<ExchangeRate> rates, IEnumerable<Currency> sourceCurrencies)
    {
        var desiredCurrencyCodes = new HashSet<string>(sourceCurrencies.Select(c => c.Code));
        return rates.Where(c => desiredCurrencyCodes.Contains(c.SourceCurrency.Code)).OrderBy(c => c.SourceCurrency.Code);
    }
}