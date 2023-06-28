namespace ExchangeRateUpdater.Models;

public class Currency
{
    public Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    public override string ToString()
    {
        return Code;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Currency currency)
        {
            return false;
        }

        return currency.Code == Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}