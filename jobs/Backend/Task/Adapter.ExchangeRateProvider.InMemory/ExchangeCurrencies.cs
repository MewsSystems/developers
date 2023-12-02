using ExchangeRateUpdater.Domain.ValueObjects;

namespace Adapter.ExchangeRateProvider.InMemory;

public class ExchangeCurrencies
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }

    public ExchangeCurrencies(Currency? sourceCurrency, Currency? targetCurrency)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ExchangeCurrencies exchangeCurrencies
            && exchangeCurrencies.SourceCurrency == SourceCurrency
            && exchangeCurrencies.TargetCurrency == TargetCurrency;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return (SourceCurrency, TargetCurrency).GetHashCode();
    }
}
