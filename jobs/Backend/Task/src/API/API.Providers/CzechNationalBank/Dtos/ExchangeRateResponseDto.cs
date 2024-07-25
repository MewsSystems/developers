using Newtonsoft.Json;

namespace Provider.CzechNationalBank.Dtos
{
    internal class ExchangeRateResponseDto
    {
        [JsonProperty("rates")]
        public required List<RateDto> Rates { get; set; }
    }
}
