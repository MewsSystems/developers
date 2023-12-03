using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

public class ExchangeResult
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public PositiveRealNumber ConvertedSum { get; }

    public ExchangeResult(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber? convertedSum)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        ConvertedSum = convertedSum ?? throw new ArgumentNullException(nameof(convertedSum));
    }
}
