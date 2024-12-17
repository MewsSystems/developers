namespace ExchangeRateUpdater.Domain;

/// <summary>
/// Represents the exchange rates information provided by the Czech National Bank.
/// </summary>
public class CnbExchangeRates
{
    public IEnumerable<CnbExchangeRate> Rates { get; set; } = new List<CnbExchangeRate>();
}

/// <summary>
/// Represents a Czech National Bank exchange rate entry.
/// </summary>
public class CnbExchangeRate
{
    public DateTimeOffset ValidFor { get; set; }

    public int Order { get; set; }

    public string Country { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public int Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal Rate { get; set; }
}