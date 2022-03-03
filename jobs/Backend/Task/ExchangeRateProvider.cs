using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.CNB.Xml;
using ExchangeRateUpdater.CNB.Xml.Elements;
using ExchangeRateUpdater.Util;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private const string DefaultCurrencyCode = "CZK";

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var xmlText = GetExchangeRateXml();
        var ratesXml = ParseXml(xmlText);
        var allRates = ConvertToExchangeRates(ratesXml);
        return FilterExchangeRates(allRates, currencies);
    }

    private static string GetExchangeRateXml()
    {
        using var client = new CNBExchangeRateClient();
        return client.GetExchangeRateXmlAsync().Result;
    }

    private static Rates ParseXml(string xmlText)
    {
        return CNBRateXmlParser.ParseXml(xmlText);
    }

    private static IEnumerable<ExchangeRate> ConvertToExchangeRates(Rates rates)
    {
        return rates?.Table?.Rows.Select(r => new ExchangeRate(new Currency(r.Code), new Currency(DefaultCurrencyCode), r.Rate.ToDecimal() / r.Amount)) ?? Enumerable.Empty<ExchangeRate>();
    }

    private static IEnumerable<ExchangeRate> FilterExchangeRates(IEnumerable<ExchangeRate> rates, IEnumerable<Currency> sourceCurrencies)
    {
        var desiredCurrencyCodes = new HashSet<string>(sourceCurrencies.Select(c => c.Code));
        return rates.Where(c => desiredCurrencyCodes.Contains(c.SourceCurrency.Code)).OrderBy(c => c.SourceCurrency.Code);
    }
}