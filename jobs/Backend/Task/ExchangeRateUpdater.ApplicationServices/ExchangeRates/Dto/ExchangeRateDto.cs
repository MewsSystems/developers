namespace ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;

/// <summary>
/// Represents the DTO of an exchange rate between two currencies.
/// </summary>
public class ExchangeRateDto
{
    public CurrencyDto SourceCurrency { get; set; } = null!;
    public CurrencyDto TargetCurrency { get; set; } = null!;
    public decimal Value { get; set; }
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}