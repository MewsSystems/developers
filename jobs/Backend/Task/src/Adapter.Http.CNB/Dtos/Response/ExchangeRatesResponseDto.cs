using Newtonsoft.Json;

namespace Adapter.Http.CNB.Dtos.Response;

public class ExchangeRatesResponseDto
{
    [JsonProperty("rates")]
    public List<ExchangeRateDto> Rates { get; set;} 
}