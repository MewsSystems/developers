namespace ExchangeRateProvider.Domain.Entities;

/// <summary>
///     Represents an exchange rate between two currencies.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ExchangeRate" /> class.
/// </remarks>
/// <param name="sourceCurrency">The source currency.</param>
/// <param name="targetCurrency">The target currency.</param>
/// <param name="value">The exchange rate value.</param>
public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, double value)
{
    /// <summary>
    ///     Gets the source currency of the exchange rate.
    /// </summary>
    public Currency SourceCurrency { get; } = sourceCurrency;

    /// <summary>
    ///     Gets the target currency of the exchange rate.
    /// </summary>
    public Currency TargetCurrency { get; } = targetCurrency;

    /// <summary>
    ///     Gets the value of the exchange rate.
    /// </summary>
    public double Value { get; } = value;

    /// <summary>
    ///     Returns a string representation of the exchange rate.
    /// </summary>
    /// <returns>A string representing the exchange rate in the format "SourceCurrency/TargetCurrency=Value".</returns>
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}
