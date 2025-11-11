namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents an exchange rate value with its multiplier.
/// Immutable value object that encapsulates the rate calculation logic.
/// </summary>
/// <remarks>
/// Exchange rates are stored as: 1 BaseCurrency = Rate/Multiplier TargetCurrency
/// For example: 1 EUR = 25.5 CZK would be stored as Rate=255, Multiplier=10
/// This allows for precise decimal representation without floating-point errors.
/// </remarks>
public record ExchangeRateValue
{
    public decimal Rate { get; }
    public int Multiplier { get; }

    /// <summary>
    /// Gets the actual exchange rate as a decimal value.
    /// </summary>
    public decimal ActualRate => Rate / Multiplier;

    private ExchangeRateValue(decimal rate, int multiplier)
    {
        Rate = rate;
        Multiplier = multiplier;
    }

    /// <summary>
    /// Creates an exchange rate value.
    /// </summary>
    /// <param name="rate">The rate numerator (must be positive)</param>
    /// <param name="multiplier">The rate denominator (must be positive, default is 1)</param>
    /// <exception cref="ArgumentException">Thrown when rate or multiplier is not positive</exception>
    public static ExchangeRateValue Create(decimal rate, int multiplier = 1)
    {
        if (rate <= 0)
            throw new ArgumentException("Rate must be positive.", nameof(rate));

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be positive.", nameof(multiplier));

        return new ExchangeRateValue(rate, multiplier);
    }

    /// <summary>
    /// Creates an exchange rate from a decimal value with automatic multiplier calculation.
    /// </summary>
    /// <param name="decimalRate">The rate as a decimal (e.g., 25.50)</param>
    /// <param name="precision">Number of decimal places to preserve (default 6)</param>
    public static ExchangeRateValue FromDecimal(decimal decimalRate, int precision = 6)
    {
        if (decimalRate <= 0)
            throw new ArgumentException("Exchange rate must be positive.", nameof(decimalRate));

        if (precision < 0 || precision > 10)
            throw new ArgumentException("Precision must be between 0 and 10.", nameof(precision));

        var multiplier = (int)Math.Pow(10, precision);
        var rate = Math.Round(decimalRate * multiplier, 0);

        return new ExchangeRateValue(rate, multiplier);
    }

    /// <summary>
    /// Calculates the inverse exchange rate.
    /// </summary>
    /// <returns>The inverse exchange rate</returns>
    public ExchangeRateValue Inverse()
    {
        // For rate R with multiplier M, inverse is M/R with multiplier 1
        // But to maintain precision, we need to adjust
        var inverseDecimal = 1m / ActualRate;
        return FromDecimal(inverseDecimal);
    }

    /// <summary>
    /// Converts an amount using this exchange rate.
    /// </summary>
    /// <param name="amount">The amount in the base currency</param>
    /// <returns>The converted amount in the target currency</returns>
    public decimal Convert(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        return amount * ActualRate;
    }

    /// <summary>
    /// Checks if this rate is within a percentage threshold of another rate.
    /// </summary>
    public bool IsWithinThreshold(ExchangeRateValue other, decimal thresholdPercent)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (thresholdPercent < 0)
            throw new ArgumentException("Threshold must be non-negative.", nameof(thresholdPercent));

        var difference = Math.Abs(ActualRate - other.ActualRate);
        var percentageDifference = (difference / ActualRate) * 100;

        return percentageDifference <= thresholdPercent;
    }

    public override string ToString() => $"{Rate}/{Multiplier} ({ActualRate:F6})";

    // Comparison operators for convenience
    public static bool operator >(ExchangeRateValue left, ExchangeRateValue right)
        => left.ActualRate > right.ActualRate;

    public static bool operator <(ExchangeRateValue left, ExchangeRateValue right)
        => left.ActualRate < right.ActualRate;

    public static bool operator >=(ExchangeRateValue left, ExchangeRateValue right)
        => left.ActualRate >= right.ActualRate;

    public static bool operator <=(ExchangeRateValue left, ExchangeRateValue right)
        => left.ActualRate <= right.ActualRate;
}
