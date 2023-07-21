using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Models;

namespace ExchangeRate.Core.Extentions;

public static class CbnExtentions
{
    public const string SourceCurrencyCode = "CZK";

    /// <summary>
    /// Maps CNB exchange rates into common ExchangeRate models.
    /// </summary>
    /// <param name="cnbExchangeRates">CNB exchange rates that should be mapped.</param>
    /// <returns>Collection of the common ExchangeRate models.</returns>
    public static IEnumerable<Models.ExchangeRate> MapToExchangeRates(this IEnumerable<CnbExchangeRate> cnbExchangeRates)
    {
        return cnbExchangeRates
            .Select(r =>
                new Models.ExchangeRate(
                    new Currency(r.CurrencyCode),
                    new Currency(SourceCurrencyCode),
                    Math.Round((decimal)r.Rate / r.Amount, 3))
                );
    }

    /// <summary>
    /// Filters ExchangeRates by currencies.
    /// </summary>
    /// <param name="exchangeRates">>CNB exchange rates that should be filtered.</param>
    /// <param name="currencies">>Collection of currencies that should be used for filtering ExchangeRates.</param>
    /// <returns>Returns collection of ExchangeRates which are present also in currency collection.</returns>
    public static IEnumerable<Models.ExchangeRate> FilterByCurrencies(this IEnumerable<Models.ExchangeRate> exchangeRates, IEnumerable<Currency> currencies)
    {
        return exchangeRates
            .Where(r =>
                currencies.Any(c => string.Equals(r.SourceCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase)));
    }

    public static bool HasAllCurrencies(this IEnumerable<Models.ExchangeRate> exchangeRates, IEnumerable<Currency> currencies)
    {
        return currencies
            .All(c =>
                exchangeRates
                    .Any(r => string.Equals(r.SourceCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase)));
    }
}
