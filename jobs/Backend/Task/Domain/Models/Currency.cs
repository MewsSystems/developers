namespace Domain.Models;
public sealed class Currency
{
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }
    public string Country { get; set; }
    public string CurrencyName { get; set; }

    public Currency(string code)
    {
        Code = code;
    }

    public Currency(string code, string country, string currencyName)
    {
        Code = code;
        Country = country;
        CurrencyName = currencyName;
    }

    public override string ToString()
    {
        return Code;
    }
}
