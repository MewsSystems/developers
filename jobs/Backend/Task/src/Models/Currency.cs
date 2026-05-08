namespace ExchangeRateUpdater;

public class Currency
{
    public string Code { get; }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
        {
            throw new ArgumentException($"Currency code must be exactly 3 characters, got '{code}'", nameof(code));
        }

        Code = code.ToUpperInvariant();
    }

    public override string ToString() => Code;
}
