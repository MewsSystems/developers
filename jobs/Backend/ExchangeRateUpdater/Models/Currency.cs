namespace ExchangeRateUpdater.Models;

public class Currency
{
    public Currency(string code) : this(default, default, default, code, default)
    { }

    public Currency(string country, string name, int amount, string code, decimal rate)
    {
        Country = country;
        Name = name;
        Amount = amount;
        Code = code;
        Rate = rate;
    }

    /// <summary>
    /// Country of the currency
    /// </summary>
    public string Country { get; }

    /// <summary>
    /// Name of the currency
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Amount of the currency.
    /// </summary>
    public int Amount { get; }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Rate of the currency
    /// </summary>
    public decimal Rate { get; }

    public override string ToString()
    {
        return Code;
    }
}

