namespace ExchangeRateUpdater.Models;

public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    /// <summary>
    /// Source currency
    /// </summary>
    public Currency SourceCurrency { get; }

    /// <summary>
    /// Target currency
    /// </summary>
    public Currency TargetCurrency { get; }

    /// <summary>
    /// Target rate
    /// </summary>
    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
