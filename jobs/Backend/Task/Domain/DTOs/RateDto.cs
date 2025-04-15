using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.DTOs;

public class RateDto
{
    [JsonPropertyName("rates")]
    public List<ExchangeRateDto> ExchangeRateDtos { get; set; }
}