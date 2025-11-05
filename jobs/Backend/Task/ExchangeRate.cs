namespace ExchangeRateUpdater;

public sealed class ExchangeRate
{
    public string SourceCurrency { get; }
    public string TargetCurrency { get; }
    public decimal Value { get; }
    public DateOnly ValidFor { get; }

    public ExchangeRate(string sourceCurrency, string targetCurrency, decimal value, DateOnly validFor)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
        ValidFor = validFor;
    }

    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}
