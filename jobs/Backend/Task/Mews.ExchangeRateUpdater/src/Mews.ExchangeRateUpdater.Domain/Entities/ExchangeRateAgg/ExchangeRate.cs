namespace Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

/// <summary>
/// Represents an exchange rate between two currencies.
/// </summary>
public class ExchangeRate : IEquatable<ExchangeRate>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sourceCurrency">The source currency of the exchange rate.</param>
    /// <param name="targetCurrency">The target currency of the exchange rate.</param>
    /// <param name="value">The value of the exchange rate, representing how much one unit of the source currency is worth in the target currency.</param>
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = value;
    }

    /// <summary>
    /// Gets the source currency of the exchange rate.
    /// </summary>
    public Currency SourceCurrency { get; }

    /// <summary>
    /// Gets the target currency of the exchange rate.
    /// </summary>
    public Currency TargetCurrency { get; }

    /// <summary>
    /// Gets the value of the exchange rate.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Converts the exchange rate to its string representation.
    /// </summary>
    /// <returns>A string representation of the exchange rate in the format "SourceCurrency/TargetCurrency=Value".</returns>
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current exchange rate.
    /// </summary>
    /// <param name="obj">The object to compare with the current exchange rate.</param>
    /// <returns>true if the specified object is an instance of <see cref="ExchangeRate"/> and equal to the current exchange rate; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ExchangeRate);
    }

    /// <summary>
    /// Determines whether another instance of <see cref="ExchangeRate"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="ExchangeRate"/> to compare with the current instance.</param>
    /// <returns>true if the specified <see cref="ExchangeRate"/> is equal to the current instance; otherwise, false.</returns>
    public bool Equals(ExchangeRate? other)
    {
        return other != null &&
               SourceCurrency.Equals(other.SourceCurrency) &&
               TargetCurrency.Equals(other.TargetCurrency) &&
               Value == other.Value;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="ExchangeRate"/>.</returns>
    public override int GetHashCode() => HashCode.Combine(SourceCurrency, TargetCurrency, Value);
}