using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

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
     
    [JsonPropertyName("code")]
    [FromQuery(Name = "code")]
    public string Code { get; set; }

    public static Currency Create(string? code)
    {
        if (code is null)
            throw new ArgumentNullException(nameof(code));

        if (code.Length != 3)
            throw new ArgumentOutOfRangeException(nameof(code), "Currency code must be a three-letter ISO 4217 code");

        return new Currency(code);
    }
    
    public static bool TryParse(string? code, out Currency currency)
    {
        try
        {
            currency = Create(code);
            return true;
        }
        catch
        {
            currency = default;
            return false;
        }
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

