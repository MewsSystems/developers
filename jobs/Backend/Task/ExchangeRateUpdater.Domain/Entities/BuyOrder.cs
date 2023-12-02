using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

public class BuyOrder
{
    public Currency SourceCurrency { get; }
    public Currency TargetCurrency { get; }
    public PositiveRealNumber SumToExchange { get; }

    public BuyOrder(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber sumToExchange)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        SumToExchange = sumToExchange ?? throw new ArgumentNullException(nameof(sumToExchange));
    }
}
