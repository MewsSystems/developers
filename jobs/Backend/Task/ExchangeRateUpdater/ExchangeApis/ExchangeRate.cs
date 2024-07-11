using ExchangeRateUpdater.RateProvider;

namespace ExchangeRateUpdater.ExchangeApis;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
{
    public Currency SourceCurrency { get; } = sourceCurrency;

    public Currency TargetCurrency { get; } = targetCurrency;

    public decimal Value { get; } = value;

    public ExchangeRate(string sourceCurrency, string targetCurrency, decimal value) :
        this(new Currency(sourceCurrency), new Currency(targetCurrency), value)
    { }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
