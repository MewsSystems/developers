using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Contract;

public readonly record struct Currency
{
    public static readonly Regex CurrencyCodeRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);

    public static readonly Currency Czk = "CZK";
    public static readonly Currency Eur = "EUR";
    public static readonly Currency Usd = "USD";
    public static readonly Currency Php = "PHP";

    public Currency(string code)
    {
        ArgumentNullException.ThrowIfNull(code);

        if (!CurrencyCodeRegex.IsMatch(code))
            throw new ArgumentException("Currency code must be a three-letter uppercase ISO 4217 code.", nameof(code));

        Code = code;
    }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    public override string ToString() => Code;

    public static implicit operator string(Currency currency) => currency.Code;
    public static implicit operator Currency(string code) => new(code);
}