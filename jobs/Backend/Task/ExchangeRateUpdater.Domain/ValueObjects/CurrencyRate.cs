namespace ExchangeRateUpdater.Domain.ValueObjects;

/// <summary>
/// This class represents currency rates.
/// </summary>
public class CurrencyRate
{
    /// <summary>
    /// Decimal representation of a currency rate.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor for CurrencyRate.
    /// </summary>
    /// <param name="value">A positive decimal value.</param>
    /// <exception cref="ArgumentOutOfRangeException">if value(currencyRate) is <= 0 will throw ArgumentOutOfRangeException</exception>
    public CurrencyRate(decimal value)
    {
        /// [2023-12-02] Victor - There is no point to have a negative rate I think. And about 0 my logic was
        ///                       what is the point of exchanging if the currency has no value?
        if (value <= 0.0m) throw new ArgumentOutOfRangeException($"{nameof(value)} has to be greater than 0.");

        Value = value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is CurrencyRate currencyRate && currencyRate.Value == this.Value;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
