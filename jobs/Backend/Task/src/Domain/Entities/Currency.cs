namespace ExchangeRateUpdater.Domain.Entities;

/// <summary>
/// Represents a currency using its ISO 4217 code.
/// </summary>
public record Currency
{
    public string Code { get; }

    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be empty", nameof(code));
        }

        if (code.Length != 3)
        {
            throw new ArgumentException("Currency code must be 3 characters", nameof(code));
        }

        Code = code.ToUpperInvariant();
    }

    public override string ToString() => Code;
}