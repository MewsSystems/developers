namespace ExchangeRateUpdater.Models;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
{
    public Currency SourceCurrency { get; } = sourceCurrency;

    public Currency TargetCurrency { get; } = targetCurrency;

    public decimal Value { get; } = value;
}