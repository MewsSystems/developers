using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CnbClient.Dtos
{
    /// <summary>
    /// Represents a response containing multiple exchange rates.
    /// </summary>
    public class ExchangeRatesResponseDto
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRateDto> Rates { get; set; } = [];
    }
}