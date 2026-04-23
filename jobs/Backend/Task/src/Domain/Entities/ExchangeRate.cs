namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
/// <remarks>
/// The value expresses how much of the target currency equals one unit of the source currency.
/// </remarks>
public record ExchangeRate
{
    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public decimal Value { get; }

    /// <summary>
    /// Creates a new exchange rate.
    /// </summary>
    /// <param name="sourceCurrency">The base currency.</param>
    /// <param name="targetCurrency">The currency being quoted.</param>
    /// <param name="value">The exchange rate value. Must be greater than zero.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sourceCurrency"/> or <paramref name="targetCurrency"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is less than or equal to zero.
    /// </exception>
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        ArgumentNullException.ThrowIfNull(sourceCurrency, nameof(sourceCurrency));
        ArgumentNullException.ThrowIfNull(targetCurrency, nameof(targetCurrency));
        if (value <= 0)
        {
            throw new ArgumentException("Exchange rate value must be positive", nameof(value));
        }

        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    public override string ToString() =>
        $"{SourceCurrency}/{TargetCurrency} = {Value}";

}
