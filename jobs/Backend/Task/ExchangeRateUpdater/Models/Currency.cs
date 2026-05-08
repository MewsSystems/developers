namespace ExchangeRateUpdater.Models;

public readonly record struct Currency
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; init; }
    
    public Currency(string code)
    {
        Code = code;
    }

    public override string ToString()
    {
        return Code;
    }
}