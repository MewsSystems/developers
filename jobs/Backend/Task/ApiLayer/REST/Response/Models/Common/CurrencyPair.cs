namespace REST.Response.Models.Common;

/// <summary>
/// Currency pair nested object for API responses.
/// </summary>
public class CurrencyPair
{
    /// <summary>
    /// Base currency code (e.g., "USD", "EUR").
    /// </summary>
    public string Base { get; set; } = string.Empty;

    /// <summary>
    /// Target currency code (e.g., "CZK", "RON").
    /// </summary>
    public string Target { get; set; } = string.Empty;
}
