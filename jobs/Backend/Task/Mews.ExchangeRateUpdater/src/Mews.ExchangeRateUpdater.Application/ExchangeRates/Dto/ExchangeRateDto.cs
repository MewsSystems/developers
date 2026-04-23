namespace Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;

/// <summary>
/// Represents the DTO of an exchange rate between two currencies.
/// </summary>
public class ExchangeRateDto : IEquatable<ExchangeRateDto>
{
    /// <summary>
    /// Gets or sets the source currency of the exchange rate.
    /// </summary>
    public CurrencyDto SourceCurrency { get; set; } = null!;

    /// <summary>
    /// Gets or sets the target currency of the exchange rate.
    /// </summary>
    public CurrencyDto TargetCurrency { get; set; } = null!;

    /// <summary>
    /// Gets or sets the value of the exchange rate.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Returns a string that represents the current exchange rate.
    /// </summary>
    /// <returns>A string that represents the current exchange rate in the format "SourceCurrency/TargetCurrency=Value".</returns>
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current exchange rate.
    /// </summary>
    /// <param name="obj">The object to compare with the current exchange rate.</param>
    /// <returns>true if the specified object is equal to the current exchange rate; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ExchangeRateDto);
    }

    /// <summary>
    /// Determines whether another instance of <see cref="ExchangeRateDto"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="ExchangeRateDto"/> to compare with the current instance.</param>
    /// <returns>true if the specified <see cref="ExchangeRateDto"/> is equal to the current instance; otherwise, false.</returns>
    public bool Equals(ExchangeRateDto? other)
    {
        if (other == null) return false;
        return SourceCurrency.Equals(other.SourceCurrency) &&
               TargetCurrency.Equals(other.TargetCurrency) &&
               Value == other.Value;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="ExchangeRateDto"/>.</returns>
    public override int GetHashCode() => HashCode.Combine(SourceCurrency, TargetCurrency, Value);
}