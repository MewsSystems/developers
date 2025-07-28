using System.Globalization;
using Mews.ExchangeRateUpdater.Domain.Base;

namespace Mews.ExchangeRateUpdater.Domain.ValueObjects;


public sealed class ExchangeRate : ValueObject
{
    public ExchangeRate(Currency source, Currency target, decimal value)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (source.Equals(target))
            throw new ArgumentException("Source and target currencies must be different.");

        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Exchange rate must be greater than zero.");

        SourceCurrency = source;
        TargetCurrency = target;
        Value = value;
    }

    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public decimal Value { get; }

    public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value.ToString(CultureInfo.InvariantCulture)}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SourceCurrency;
        yield return TargetCurrency;
        yield return Value;
    }
}