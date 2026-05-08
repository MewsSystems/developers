namespace CurrencyExchange.Model;

public class Currency(string code)
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; } = code;

    public override string ToString()
    {
        return Code;
    }

    public override bool Equals(object? obj)
    {
        return obj is Currency currency && currency.Code.Equals(Code, StringComparison.Ordinal);
    }
}
