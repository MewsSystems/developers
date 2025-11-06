namespace ApplicationLayer.DTOs.Currencies;

/// <summary>
/// DTO for currency information.
/// Note: Currency is a value object with minimal properties (Id, Code).
/// Additional properties are optional and may not be available from all sources.
/// </summary>
public class CurrencyDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Symbol { get; set; }
    public int? DecimalPlaces { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? Created { get; set; }
}
