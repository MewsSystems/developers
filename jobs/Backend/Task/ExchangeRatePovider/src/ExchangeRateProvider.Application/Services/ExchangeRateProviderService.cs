using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Application.Services;

/// <summary>
/// Implementation of the exchange rate provider service that filters exchange rates by requested currencies.
/// </summary>
/// <param name="exchangeRateProvider">The underlying exchange rate provider for retrieving all available exchange rates.</param>
public class ExchangeRateProviderService(
    IExchangeRateProvider exchangeRateProvider) : IExchangeRateProviderService
{
    /// <summary>
    /// Retrieves the latest exchange rates for the specified currencies by fetching all available
    /// rates and filtering them.
    /// </summary>
    /// <param name="currencies">The collection of currencies for which to retrieve exchange rates.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only
    /// list of exchange rates filtered by the requested currencies.</returns>
    public async Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default)
    {
        var exchangeRates = await exchangeRateProvider.GetLatestAsync(cancellationToken);

        return FilterByCurrencies(exchangeRates, currencies);
    }

    /// <summary>
    /// Filters exchange rates to include only those whose source currency matches the requested currencies.
    /// </summary>
    /// <param name="exchangeRates">The complete collection of exchange rates to filter.</param>
    /// <param name="currencies">The collection of currencies to filter by.</param>
    /// <returns>A list of exchange rates that match the requested source currencies. Returns an empty list
    /// if no currencies are requested.</returns>
    private static List<ExchangeRate> FilterByCurrencies(
        IEnumerable<ExchangeRate> exchangeRates,
        IEnumerable<Currency> currencies)
    {
        var requested = new HashSet<Currency>(currencies);

        if (requested.Count == 0)
        {
            return [];
        }

        return [.. exchangeRates.Where(r => requested.Contains(r.SourceCurrency))];
    }
}
