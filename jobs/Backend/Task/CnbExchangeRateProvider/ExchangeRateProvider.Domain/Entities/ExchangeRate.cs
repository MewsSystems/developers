using System.Globalization;

namespace ExchangeRateProvider.Domain.Entities;

/// <summary>
/// Represents an exchange rate between two currencies.
/// This is a value object that encapsulates the rate information and provides validation.
/// </summary>
public sealed class ExchangeRate : IEquatable<ExchangeRate>
{
    /// <summary>
    /// Gets the source currency of the exchange rate.
    /// </summary>
    public Currency SourceCurrency { get; set; }

    /// <summary>
    /// Gets the target currency of the exchange rate.
    /// </summary>
    public Currency TargetCurrency { get; set; }

    /// <summary>
    /// Gets the exchange rate value (how many target currency units per source currency unit).
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Gets the timestamp when this rate was created or retrieved.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Parameterless constructor for JSON deserialization.
    /// </summary>
    public ExchangeRate()
    {
        SourceCurrency = new Currency();
        TargetCurrency = new Currency();
        Value = 1;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the ExchangeRate class.
    /// </summary>
    /// <param name="sourceCurrency">The source currency.</param>
    /// <param name="targetCurrency">The target currency.</param>
    /// <param name="value">The exchange rate value.</param>
    /// <param name="timestamp">The timestamp of the rate (defaults to current time).</param>
    /// <exception cref="InvalidExchangeRateException">Thrown when the exchange rate value is invalid.</exception>
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime? timestamp = null)
    {
        if (sourceCurrency == null) throw new ArgumentNullException(nameof(sourceCurrency));
        if (targetCurrency == null) throw new ArgumentNullException(nameof(targetCurrency));

        if (value <= 0)
        {
            throw new InvalidExchangeRateException("Exchange rate value must be positive.");
        }

        if (value > 1_000_000)
        {
            throw new InvalidExchangeRateException("Exchange rate value seems unreasonably high.");
        }

        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Value = Math.Round(value, 6); // Standard precision for exchange rates
        Timestamp = timestamp ?? DateTime.UtcNow;
    }


    /// <summary>
    /// Returns a string representation of the exchange rate.
    /// </summary>
    public override string ToString() =>
        $"{SourceCurrency}/{TargetCurrency}={Value.ToString("0.######", CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Determines whether the specified object is equal to the current exchange rate.
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as ExchangeRate);

    /// <summary>
    /// Determines whether the specified exchange rate is equal to the current exchange rate.
    /// </summary>
    public bool Equals(ExchangeRate? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return SourceCurrency == other.SourceCurrency &&
               TargetCurrency == other.TargetCurrency &&
               Value == other.Value;
    }

    /// <summary>
    /// Returns a hash code for the exchange rate.
    /// </summary>
    public override int GetHashCode() =>
        HashCode.Combine(SourceCurrency, TargetCurrency, Value);

    /// <summary>
    /// Determines whether two exchange rates are equal.
    /// </summary>
    public static bool operator ==(ExchangeRate? left, ExchangeRate? right) => Equals(left, right);

    /// <summary>
    /// Determines whether two exchange rates are not equal.
    /// </summary>
    public static bool operator !=(ExchangeRate? left, ExchangeRate? right) => !Equals(left, right);

}

/// <summary>
/// Exception thrown when an invalid exchange rate is provided.
/// </summary>
public class InvalidExchangeRateException : DomainException
{
    public InvalidExchangeRateException(string message) : base(message) { }
}
