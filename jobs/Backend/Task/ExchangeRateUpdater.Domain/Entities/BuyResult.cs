

using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

public class BuyResult
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public PositiveRealNumber ConvertedSum { get; }

    public BuyResult(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber? convertedSum)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        ConvertedSum = convertedSum ?? throw new ArgumentNullException(nameof(convertedSum));
    }
}
