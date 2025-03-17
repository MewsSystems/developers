using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.DTOs;

public class ExchangeRateListDto
{
    [JsonPropertyName("rates")]
    public IEnumerable<ExchangeRateDto> Rates { get; set; }
}
