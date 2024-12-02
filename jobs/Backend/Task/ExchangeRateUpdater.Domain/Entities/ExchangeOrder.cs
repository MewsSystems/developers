using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// This class cpntains the exchange order information.
/// </summary>
public class ExchangeOrder
{
    /// <summary>
    /// Currency from which the exhange will happen.
    /// </summary>
    public Currency SourceCurrency { get; }
    /// <summary>
    /// Currency to which the transfer will happen.
    /// </summary>
    public Currency TargetCurrency { get; }
    /// <summary>
    /// A number representing the sum to be exchanged.
    /// This number is specified in SourceCurrency.
    /// </summary>
    public PositiveRealNumber SumToExchange { get; }

    /// <summary>
    /// The constructor for the ExchangeOrder.
    /// </summary>
    /// <param name="sourceCurrency">Currency that needs to be exchanged.</param>
    /// <param name="targetCurrency">Currency that the exchange will happen in.</param>
    /// <param name="sumToExchange">The sum to be exchanged.</param>
    /// <exception cref="ArgumentNullException">sourceCountry, targetCountry, and sumToExchange can't be null.</exception>
    public ExchangeOrder(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber sumToExchange)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        SumToExchange = sumToExchange ?? throw new ArgumentNullException(nameof(sumToExchange));
    }
}
