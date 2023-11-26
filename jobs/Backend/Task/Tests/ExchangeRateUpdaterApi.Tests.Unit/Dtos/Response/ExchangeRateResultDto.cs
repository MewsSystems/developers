using ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;
using Newtonsoft.Json;

namespace ExchangeRateUpdaterApi.Tests.Unit.Dtos.Response;

public class ExchangeRateResultDto
{
    [JsonProperty("sourceCurrency")]
    public CurrencyDto SourceCurrency { get; set; }
    
    [JsonProperty("targetCurrency")]
    public CurrencyDto TargetCurrency { get; set; }
    
    [JsonProperty("value")]
    public decimal Value { get; set; }
}