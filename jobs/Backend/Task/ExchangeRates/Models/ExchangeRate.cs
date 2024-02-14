using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRatesService.Models;

[JsonSerializable(typeof(ExchangeRate))]
public class ExchangeRate
{
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, int amount)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency ?? new Currency("CZK");
        Value =  (amount != 1) ? value / amount : value;
        Amount = amount;
    }

    //[JsonConverter(typeof(CurrencyJsonConverter))]
    //[JsonPropertyName("currencyCode")]
    public Currency SourceCurrency { get; private set; }

    public Currency TargetCurrency { get; private set; }

    [JsonPropertyName("rate")]
    public decimal Value { get; }

    [JsonPropertyName("amount")]
    public int Amount { get; }

    public override string ToString()
    {
        return $"{SourceCurrency.Code}/{TargetCurrency.Code}={Value}";
    }
        
    public override bool Equals(object obj)
    {
        ExchangeRate rate = (ExchangeRate) obj;

        return (SourceCurrency.Code == rate.SourceCurrency.Code)
               && (TargetCurrency.Code == rate.TargetCurrency.Code)
               && (Value == rate.Value);
    }
        
    public override int GetHashCode()
    {
        return HashCode.Combine(SourceCurrency.Code, TargetCurrency.Code, Value);
    }
}
    
public class CurrencyJsonConverter: JsonConverter<Currency>
{
    public override Currency Read(
        ref Utf8JsonReader reader,
        Type Currency,
        JsonSerializerOptions options)
    {
        var currency = System.Text.Encoding.UTF8.GetString(reader.ValueSpan);
        return new Currency(currency);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Currency currency,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(currency.ToString());
}