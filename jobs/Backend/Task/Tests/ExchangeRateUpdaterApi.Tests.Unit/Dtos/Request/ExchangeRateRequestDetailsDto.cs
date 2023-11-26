using Newtonsoft.Json;

namespace ExchangeRateUpdaterApi.Tests.Unit.Dtos.Request;

public class ExchangeRateRequestDetailsDto
{
    [JsonProperty("sourceCurrency")]
    public CurrencyDto SourceCurrency { get; set; }
    
    [JsonProperty("targetCurrency")]
    public CurrencyDto TargetCurrency { get; set; }
}