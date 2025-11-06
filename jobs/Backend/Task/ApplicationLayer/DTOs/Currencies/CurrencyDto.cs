namespace ApplicationLayer.DTOs.Currencies;

/// <summary>
/// DTO for currency information.
/// Currency is a value object with minimal properties (Id, Code).
/// </summary>
public class CurrencyDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
}
