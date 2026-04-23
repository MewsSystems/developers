namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Represents a collection of exchange rates as returned by the Czech National Bank (CNB) API.
/// </summary>
internal record CnbExchangeRates
{
    /// <summary>
    /// A collection of exchange rate entries from the CNB API response.
    /// </summary>
    public required IEnumerable<CnbExchangeRate> Rates { get; init; }
}
