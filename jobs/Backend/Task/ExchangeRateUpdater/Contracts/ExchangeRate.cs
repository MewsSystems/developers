namespace ExchangeRateUpdater.Contracts;

/// <summary>
/// Exchange rate between two currencies.
/// </summary>
public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    /// <summary>
    /// Source currency.
    /// </summary>
    public Currency SourceCurrency { get; }

    /// <summary>
    /// Target currency.
    /// </summary>
    public Currency TargetCurrency { get; }

    /// <summary>
    /// Exchange rate, i.e. how many units of <see cref="TargetCurrency"/> is single unit of <see cref="SourceCurrency"/> worth.
    /// </summary>
    public decimal Value { get; }

    public override string ToString()
    {
        return $"{SourceCurrency.Code},{TargetCurrency.Code},{Value}";
    }
}