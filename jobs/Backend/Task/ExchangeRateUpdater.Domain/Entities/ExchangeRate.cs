using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// This class holds all necessary information reagrding an exchange rate.
/// </summary>
public class ExchangeRate
{
    /// <summary>
    /// The original currency which the conversion happens from.
    /// </summary>
    public Currency SourceCurrency { get; }
    /// <summary>
    /// The currency which the conversion happens to.
    /// </summary>
    public Currency TargetCurrency { get; }
    /// <summary>
    /// The Exchange Rate between two currencies.
    /// </summary>
    public PositiveRealNumber CurrencyRate { get; }
    /// <summary>
    /// The date of the exchange rate.
    /// </summary>
    public DateTime RateDate{ get; }

    /// <summary>
    /// The constructor for ExchangeRate.
    /// </summary>
    /// <param name="sourceCurrency">Currency that needs to be converted.</param>
    /// <param name="targetCurrency">Currency in which the conversion will happen.</param>
    /// <param name="currencyRate">The rate of the exchange between the two currencies</param>
    /// <param name="rateDate">The date of the exchange rate</param>
    /// <exception cref="ArgumentNullException">sourceCurrency, targetCurrency,currencyRate cannot be null.</exception>
    public ExchangeRate(Currency? sourceCurrency, Currency? targetCurrency, PositiveRealNumber? currencyRate, DateTime rateDate)
    {
        SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));
        TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
        CurrencyRate = currencyRate ?? throw new ArgumentNullException(nameof(currencyRate));
        RateDate = rateDate;
    }
}
