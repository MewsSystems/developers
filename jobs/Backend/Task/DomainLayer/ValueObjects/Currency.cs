namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents a currency as an immutable value object.
/// Uses ISO 4217 three-letter currency codes.
/// </summary>
public record Currency
{
    private static readonly HashSet<string> ValidCurrencyCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "USD", "EUR", "CZK", "GBP", "JPY", "KRW", "CNY", "AUD", "CAD", "CHF",
        "HKD", "NZD", "SEK", "KRW", "SGD", "NOK", "MXN", "INR", "RUB", "ZAR",
        "TRY", "BRL", "TWD", "DKK", "PLN", "THB", "IDR", "HUF", "ISK", "PHP",
        "MYR", "RON", "BGN", "HRK", "ILS"
    };

    public string Code { get; }

    private Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Creates a Currency from a three-letter ISO 4217 code.
    /// </summary>
    /// <param name="code">The ISO 4217 currency code (e.g., "USD", "EUR")</param>
    /// <returns>A Currency value object</returns>
    /// <exception cref="ArgumentException">Thrown when the code is invalid</exception>
    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        var normalizedCode = code.Trim().ToUpperInvariant();

        if (normalizedCode.Length != 3)
            throw new ArgumentException($"Currency code must be exactly 3 characters. Got: '{code}'", nameof(code));

        if (!ValidCurrencyCodes.Contains(normalizedCode))
            throw new ArgumentException($"Invalid or unsupported currency code: '{code}'", nameof(code));

        return new Currency(normalizedCode);
    }

    /// <summary>
    /// Attempts to create a Currency from a code without throwing exceptions.
    /// </summary>
    public static bool TryFromCode(string code, out Currency? currency)
    {
        try
        {
            currency = FromCode(code);
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
        return normalizedCode.Length == 3 && ValidCurrencyCodes.Contains(normalizedCode);
    }

    public override string ToString() => Code;

    // Common currencies as static properties for convenience
    public static Currency USD => new("USD");
    public static Currency EUR => new("EUR");
    public static Currency CZK => new("CZK");
    public static Currency GBP => new("GBP");
    public static Currency JPY => new("JPY");
}
