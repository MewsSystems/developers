namespace ExchangeRateUpdater.Api.Domain;

using Common;

/// <summary>
/// Represents a currency identified by its ISO 4217 three-letter code.
/// </summary>
public sealed record Currency
{
    private Currency(string code)
    {
        Code = code.ToUpperInvariant();
    }

    /// <summary>
    /// Three-letter ISO 4217 currency code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Creates a new Currency instance with validation.
    /// </summary>
    public static Result<Currency> Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result<Currency>.Failure(CurrencyErrors.NullOrWhitespace());

        if (code.Length != 3)
            return Result<Currency>.Failure(CurrencyErrors.InvalidLength(code));

        if (!code.All(char.IsLetter))
            return Result<Currency>.Failure(CurrencyErrors.InvalidCharacters(code));

        return Result<Currency>.Success(new Currency(code));
    }

    public override string ToString() => Code;
}
