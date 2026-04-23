namespace ExchangeRateProvider.Infrastructure.ExchangeRateProviders.Cnb;

/// <summary>
/// Represents an exchange rate data structure as returned by the Czech National Bank (CNB) API.
/// </summary>
internal record CnbExchangeRate
{
    /// <summary>
    /// The amount of the foreign currency that corresponds to the exchange rate.
    /// </summary>
    public int Amount { get; init; }

    /// <summary>
    /// The country associated with the currency.
    /// </summary>
    public required string Country { get; init; }

    /// <summary>
    /// The full name of the currency.
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// The three-letter ISO 4217 currency code.
    /// </summary>
    public required string CurrencyCode { get; init; }

    /// <summary>
    /// The order number used by CNB for sorting currencies.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The exchange rate value in Czech koruna (CZK) for the specified amount of foreign currency.
    /// </summary>
    public decimal Rate { get; init; }

    /// <summary>
    /// The date and time for which this exchange rate is valid.
    /// </summary>
    public DateOnly ValidFor { get; init; }
}
