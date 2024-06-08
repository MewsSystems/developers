namespace CurrencyExchange.Model;

/// <summary>
/// Represents a daily exchange rate between source currency (not specified here) and target currency.
/// </summary>
/// <remarks>There are several properties returned from the CNB REST API that are ignored as they are not necessary for the application.</remarks>
public class DailyRate
{
    /// <summary>
    /// Number of currency units the rate is defined for. 
    /// </summary>
    public required int Amount { get; init; }

    /// <summary>
    /// ISO Code of the currency.
    /// </summary>
    public required string CurrencyCode { get; init; }

    /// <summary>
    /// Price the target currency is sold for in the source currency. 
    /// </summary>
    public required decimal Rate { get; init; }

    /// <summary>
    /// Date the rate is valid for.
    /// </summary>
    public required DateTimeOffset ValidFor { get; init; }
}