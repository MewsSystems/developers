namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents a currency as an immutable value object.
/// Uses ISO 4217 three-letter currency codes.
/// Note: Contains an Id for persistence purposes while maintaining value object semantics.
/// </summary>
public record Currency
{
    public int Id { get; init; }
    public string Code { get; }

    private Currency(string code, int id = 0)
    {
        Code = code;
        Id = id;
    }

    /// <summary>
    /// Creates a Currency from a three-letter ISO 4217 code.
    /// </summary>
    /// <param name="code">The ISO 4217 currency code (e.g., "USD", "EUR")</param>
    /// <param name="id">Optional database identifier</param>
    /// <returns>A Currency value object</returns>
    /// <exception cref="ArgumentException">Thrown when the code is invalid</exception>
    public static Currency FromCode(string code, int id = 0)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        var normalizedCode = code.Trim().ToUpperInvariant();

        if (normalizedCode.Length != 3)
            throw new ArgumentException($"Currency code must be exactly 3 characters. Got: '{code}'", nameof(code));

        return new Currency(normalizedCode, id);
    }

    /// <summary>
    /// Attempts to create a Currency from a code without throwing exceptions.
    /// </summary>
    public static bool TryFromCode(string code, out Currency? currency, int id = 0)
    {
        try
        {
            currency = FromCode(code, id);
            return true;
        }
        catch
        {
            currency = null;
            return false;
        }
    }

    /// <summary>
    /// Checks if a currency code is valid.
    /// </summary>
    public static bool IsValid(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        var normalizedCode = code.Trim().ToUpperInvariant();
        return normalizedCode.Length == 3;
    }

    public override string ToString() => Code;
}
