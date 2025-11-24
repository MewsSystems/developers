namespace ExchangeRateUpdater;

public class ExchangeRate
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public decimal Value { get; }

    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));

        if (value <= 0)
        {
            throw new ArgumentException("Exchange rate must be greater than zero", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
}
