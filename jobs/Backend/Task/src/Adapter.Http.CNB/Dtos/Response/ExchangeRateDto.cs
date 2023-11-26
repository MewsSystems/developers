using Newtonsoft.Json;

namespace Adapter.Http.CNB.Dtos.Response;

public class ExchangeRateDto
{
    [JsonProperty("validFor")]
    public DateTime ValidFor { get; set; }
    
    [JsonProperty("currencyCode")]
    public string TargetCurrency { get; set; }
    
    [JsonProperty("rate")]
    public decimal Rate { get; set; }
}