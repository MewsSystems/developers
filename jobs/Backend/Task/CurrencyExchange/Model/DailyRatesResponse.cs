namespace CurrencyExchange.Model;

/// <summary>
/// Represents a root response from the external exchange rate source.
/// </summary>
public class DailyRatesResponse
{
    /// <summary>
    /// Collection of rates for different currencies.
    /// </summary>
    public required IEnumerable<DailyRate> Rates { get; set; }
}