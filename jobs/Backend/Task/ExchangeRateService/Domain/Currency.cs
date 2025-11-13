using System.Globalization;

namespace ExchangeRateService.Domain;

public readonly struct Currency(string code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    private string Code { get; } = code.ToUpper(CultureInfo.InvariantCulture);
    
    public override string ToString() => Code;
}