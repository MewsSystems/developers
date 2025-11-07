namespace REST.Response.Models.Areas.Currencies;

/// <summary>
/// API response model for currency information.
/// </summary>
public class CurrencyResponse
{
    /// <summary>
    /// Currency unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Currency code (ISO 4217 format, e.g., "USD", "EUR", "CZK").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Currency name (e.g., "US Dollar", "Euro", "Czech Koruna").
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Currency symbol (e.g., "$", "€", "Kč").
    /// </summary>
    public string? Symbol { get; set; }
}
