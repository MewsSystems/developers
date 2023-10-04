using ExchangeRateUpdater;

namespace ExchangeRatesUpdater.Common;

public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
{
    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value:F8}";
}
