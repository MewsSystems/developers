namespace ExchangeRateProvider.Host.WebApi.Models;

using Domain.Entities;

/// <summary>
///     Response model for fetching exchange rates.
/// </summary>
public class GetExchangeRatesResponse
{
    /// <summary>
    ///     List of successfully resolved exchange rates.
    /// </summary>
    public required IEnumerable<ExchangeRate> Rates { get; set; }

    /// <summary>
    ///     List of currency codes that could not be resolved.
    ///     These are the codes that were requested but not found in the data source.
    /// </summary>
    public required IEnumerable<string> CurrenciesNotResolved { get; set; }
}
