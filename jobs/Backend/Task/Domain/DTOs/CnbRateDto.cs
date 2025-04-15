using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.DTOs;

public class CnbRateDto
{
    [JsonPropertyName("rates")]
    public List<CnbExchangeRateDto> ExchangeRateDtos { get; set; }
}