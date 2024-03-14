namespace ExchangeRateService.Domain;

internal readonly struct Currency(string code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    private string Code { get; } = code;
    
    public override string ToString() => Code;
}