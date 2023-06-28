using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Clients.Models;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankExchangeRateClient _cnbExchangeRateClient;

    public CzechNationalBankExchangeRateProvider(ICzechNationalBankExchangeRateClient cnbExchangeRateClient)
    {
        _cnbExchangeRateClient = cnbExchangeRateClient;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var cnbExchangeRates = _cnbExchangeRateClient.GetDailyRates();
        var exchangeRates = MapCnbExchangeRateToExchangeRate(cnbExchangeRates);
        var filteredExchangeRates = FilterRatesByCurrencies(currencies, exchangeRates);
        return filteredExchangeRates.ToList();
    }

    private static IEnumerable<ExchangeRate> FilterRatesByCurrencies(IEnumerable<Currency> currencies,
        IEnumerable<ExchangeRate> exchangeRates) =>
        exchangeRates.Where(x => currencies.Contains(x.TargetCurrency));

    private static IEnumerable<ExchangeRate> MapCnbExchangeRateToExchangeRate(CnbExchangeRates cnbExchangeRates)
    {
        var result = new List<ExchangeRate>();
        var sourceCurrency = new Currency("CZK");
        foreach (var cnbExchangeRate in cnbExchangeRates.Rates)
        {
            var targetCurrency = new Currency(cnbExchangeRate.CurrencyCode);
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, cnbExchangeRate.Rate);
            result.Add(exchangeRate);
        }

        return result;
    }
}