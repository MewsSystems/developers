using System.Text.Json.Serialization;

namespace ExchangeRatesService.Models;

[JsonSerializable(typeof(Currency))]
public class Currency
{
    [JsonConstructor]
    public Currency(string code)
    {
        Code = code;
    }
    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
     
    [JsonPropertyName("currencyCode")]
    public string Code { get; }

    public static Currency Create(string? code)
    {
        if (code is null)
            throw new ArgumentNullException(nameof(code));

        if (code.Length != 3)
            throw new ArgumentOutOfRangeException(nameof(code), "Currency code must be a three-letter ISO 4217 code");

        return new Currency(code);
    }
    
    public override string ToString()
    {
        return Code;
    }
    
    public override bool Equals(object obj)
    {
        Currency currency = (Currency) obj;
        return (Code == currency.Code);
    }
    
    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}

