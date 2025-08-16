using System.Text.Json.Serialization;

public class ExchangeRate
{
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

[JsonSerializable(typeof(ExchangeRate))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{

}