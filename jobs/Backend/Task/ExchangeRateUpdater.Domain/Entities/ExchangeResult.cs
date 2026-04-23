using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// The class containing the result of an exchange operation.
/// </summary>
public class ExchangeResult
{
    /// <summary>
    /// The currency which was exchanged.
    /// </summary>
    public Currency SourceCurrency { get; }
    /// <summary>
    /// The currency to which the exchanged happened.
    /// </summary>
    public Currency TargetCurrency { get; }
    /// <summary>
    /// The exchanged sum specified in target currency.
    /// </summary>
    public PositiveRealNumber ConvertedSum { get; }
    /// <summary>
    /// The exchange rate date.
    /// </summary>
    public DateTime RateDate { get; }

    /// <summary>
    /// The constructor for the ExchangeResult.
    /// </summary>
    /// <param name="sourceCurrency">Currency that was exchanged.</param>
    /// <param name="targetCurrency">Currency that the exchange happened tp.</param>
    /// <param name="convertedSum">The sum that was converted.</param>
    /// <exception cref="ArgumentNullException">sourceCountry, targetCountry, and convertedSum can't be null.</exception>
    public ExchangeResult(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber? convertedSum, DateTime rateDate)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        ConvertedSum = convertedSum ?? throw new ArgumentNullException(nameof(convertedSum));
        RateDate = rateDate;
    }
}
