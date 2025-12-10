using ExchangeRateUpdater.Common.Models;
using ExchangeRateUpdater.Features.ExchangeRates.Models;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRateResponse> sourceRates);
}
public sealed class ExchangeRateProvider : IExchangeRateProvider
{
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, IEnumerable<ExchangeRateResponse> sourceRates)
    {
        var currencyCodes = currencies.Select(x => x.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

        return sourceRates
                .Where(rate => rate.CurrencyCode != null && currencyCodes.Contains(rate.CurrencyCode))
                .Select(rate =>
                    new ExchangeRate(
                        new Currency(rate.CurrencyCode!),
                        new Currency("CZK"),
                        rate.Rate))
                .ToList();
    }
}
