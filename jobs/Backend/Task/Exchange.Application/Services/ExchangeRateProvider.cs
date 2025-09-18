using Exchange.Application.Abstractions.ApiClients;
using Exchange.Domain.Entities;
using Exchange.Domain.ValueObjects;

namespace Exchange.Application.Services;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default
    );
}

public class ExchangeRateProvider(ICnbApiClient cnbApiClient) : IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default
    )
    {
        var requestedCurrencyCodes = new HashSet<string>(
            currencies.Select(c => c.Code),
            StringComparer.OrdinalIgnoreCase
        );

        var allCnbExchangeRates = await cnbApiClient.GetExchangeRatesAsync(cancellationToken);

        var filteredExchangeRates = allCnbExchangeRates
            .Where(r => requestedCurrencyCodes.Contains(r.CurrencyCode))
            .Select(rate => new ExchangeRate(
                Currency.CZK,
                Currency.FromCode(rate.CurrencyCode),
                (decimal)rate.Amount / (decimal)rate.Rate
            ))
            .ToList();

        return filteredExchangeRates;
    }
}