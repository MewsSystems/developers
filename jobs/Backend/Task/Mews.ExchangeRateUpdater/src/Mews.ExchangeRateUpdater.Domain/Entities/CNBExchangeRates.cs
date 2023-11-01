namespace Mews.ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// Represents the exchange rates information provided by the Czech National Bank.
/// </summary>
public class CNBExchangeRates
{
    /// <summary>
    /// Gets or sets the collection of exchange rate details.
    /// </summary>
    public IEnumerable<CNBExchangeRate> Rates { get; set; } = new List<CNBExchangeRate>();
}

/// <summary>
/// Represents a single exchange rate entry as provided by the Czech National Bank.
/// </summary>
public class CNBExchangeRate
{
    /// <summary>
    /// Gets or sets the date and time when the exchange rate is valid.
    /// </summary>
    public DateTimeOffset ValidFor { get; set; }

    /// <summary>
    /// Gets or sets the order of the currency.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the name of the country associated with the currency.
    /// </summary>
    public string Country { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the currency.
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Gets or sets the amount for which the exchange rate is given.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Gets or sets the three-letter ISO 4217 code of the currency.
    /// </summary>
    public string CurrencyCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets the exchange rate of the currency.
    /// </summary>
    public decimal Rate { get; set; }
}