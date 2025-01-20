using Newtonsoft.Json;

namespace ExchangeRateUpdater.Data.Responses;
public class ExchangeRatesResponseDto
{
    [JsonProperty("rates")]
    public List<ExchangeRateDto> Rates { get; set; }
}
