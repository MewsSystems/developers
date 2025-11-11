namespace REST.Request.Models.Areas.Currencies;

/// <summary>
/// Request model for creating a currency.
/// </summary>
public class CreateCurrencyRequest
{
    /// <summary>
    /// Currency code (ISO 4217).
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
