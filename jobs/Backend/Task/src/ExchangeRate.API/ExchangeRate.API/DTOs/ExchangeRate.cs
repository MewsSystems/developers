namespace ExchangeRate.API.DTOs;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
{
    public Currency SourceCurrency { get; } = sourceCurrency;

    public Currency TargetCurrency { get; } = targetCurrency;

    public decimal Value { get; } = value;

    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}