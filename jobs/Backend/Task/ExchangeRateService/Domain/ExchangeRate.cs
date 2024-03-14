namespace ExchangeRateService.Domain;

public record ExchangeRate
{
    public required Currency SourceCurrency { get; init; }
    public required Currency TargetCurrency { get; init; }
    public required float Value { get; init; }
    
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}