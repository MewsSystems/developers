namespace ExchangeRateUpdater.Core.Models;

public class Currency(string code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code => code;

    public override string ToString()
    {
        return Code;
    }
}