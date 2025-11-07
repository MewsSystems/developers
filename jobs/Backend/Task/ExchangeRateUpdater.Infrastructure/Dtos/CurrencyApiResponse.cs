using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.Dtos;

public record CurrencyApiResponse()
{
    [JsonPropertyName("data")] 
    public Dictionary<string, CurrencyApiRate> Data { get; set; }
};

public record CurrencyApiRate
{
    [JsonPropertyName("code")] 
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("value")]
    public decimal Value { get; init; }
}