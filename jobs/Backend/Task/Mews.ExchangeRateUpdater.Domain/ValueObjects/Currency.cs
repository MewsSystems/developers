using System.Text.RegularExpressions;
using Mews.ExchangeRateUpdater.Domain.Base;

namespace Mews.ExchangeRateUpdater.Domain.ValueObjects;

public sealed class Currency : ValueObject
{
    private static readonly Regex CodeRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            throw new ArgumentException("Currency code must be a 3-letter ISO 4217 string.", nameof(code));

        code = code.ToUpperInvariant();

        if (!CodeRegex.IsMatch(code))
            throw new ArgumentException("Currency code must contain only 3 uppercase letters (A-Z).", nameof(code));
        
        Code = code;
    }

    public string Code { get; }

    public override string ToString() => Code;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}