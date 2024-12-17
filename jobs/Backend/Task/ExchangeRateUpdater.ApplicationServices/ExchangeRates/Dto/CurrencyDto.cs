namespace ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;

/// <summary>
/// Represents the DTO of a Three-letter ISO 4217 currency code.
/// </summary>
public class CurrencyDto
{
    public string Code { get; set; } = null!;
    public override string ToString()
    {
        return Code;
    }
}