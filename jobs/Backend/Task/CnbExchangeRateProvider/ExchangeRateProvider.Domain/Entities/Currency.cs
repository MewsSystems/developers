using System.Text.RegularExpressions;

namespace ExchangeRateProvider.Domain.Entities;

/// <summary>
/// Represents a currency with ISO 4217 standard validation.
/// This is a value object that encapsulates currency code validation and behavior.
/// </summary>
public sealed class Currency : IEquatable<Currency>
{
    private static readonly Regex CurrencyCodePattern = new(@"^[A-Z]{3}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the ISO 4217 currency code (3 uppercase letters).
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Parameterless constructor for JSON deserialization.
    /// </summary>
    public Currency() { Code = "XXX"; }

    /// <summary>
    /// Initializes a new instance of the Currency class.
    /// </summary>
    /// <param name="code">The ISO 4217 currency code.</param>
    /// <exception cref="InvalidCurrencyCodeException">Thrown when the currency code is invalid.</exception>
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new InvalidCurrencyCodeException("Currency code cannot be null or whitespace.");
        }

        if (!CurrencyCodePattern.IsMatch(code.ToUpperInvariant()))
        {
            throw new InvalidCurrencyCodeException($"Currency code '{code}' must be exactly 3 alphabetic characters.");
        }

        Code = code.ToUpperInvariant();
    }


    /// <summary>
    /// Returns a string representation of the currency.
    /// </summary>
    public override string ToString() => Code;

    /// <summary>
    /// Determines whether the specified object is equal to the current currency.
    /// </summary>
    public override bool Equals(object? obj) => Equals(obj as Currency);

    /// <summary>
    /// Determines whether the specified currency is equal to the current currency.
    /// </summary>
    public bool Equals(Currency? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Code.Equals(other.Code, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a hash code for the currency.
    /// </summary>
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Code);

    /// <summary>
    /// Determines whether two currencies are equal.
    /// </summary>
    public static bool operator ==(Currency? left, Currency? right) => Equals(left, right);

    /// <summary>
    /// Determines whether two currencies are not equal.
    /// </summary>
    public static bool operator !=(Currency? left, Currency? right) => !Equals(left, right);

    /// <summary>
    /// Creates a currency from a string, returning null if invalid.
    /// </summary>
    public static Currency? TryCreate(string code)
    {
        try
        {
            return new Currency(code);
        }
        catch (InvalidCurrencyCodeException)
        {
            return null;
        }
    }
}

/// <summary>
/// Exception thrown when an invalid currency code is provided.
/// </summary>
public class InvalidCurrencyCodeException : DomainException
{
    public InvalidCurrencyCodeException(string message) : base(message) { }
}

/// <summary>
/// Base class for domain-specific exceptions.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}
