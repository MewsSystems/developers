namespace ExchangeRateUpdater.Domain.Models;

/// <summary>
/// Currency with a three-letter ISO 4217 code.
/// </summary>
public record Currency(string Code)
{
    public string Code { get; } = ValidateAndNormalizeCode(Code);

    private static string ValidateAndNormalizeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty", nameof(code));

        if (code.Length != 3)
            throw new ArgumentException("Currency code must be exactly 3 characters", nameof(code));

        return code.ToUpperInvariant();
    }

    public override string ToString()
    {
        return Code;
    }
}
