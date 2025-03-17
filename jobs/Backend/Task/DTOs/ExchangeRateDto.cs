using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.DTOs;

public class ExchangeRateDto
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }
    [JsonPropertyName("order")]
    public int Order { get; set; }
    [JsonPropertyName("counntry")]
    public string Country { get; set; }
}
