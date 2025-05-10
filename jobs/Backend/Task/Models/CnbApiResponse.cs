using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models;

public class CnbApiResponse
{
    [JsonPropertyName("rates")]
    public List<CnbExchangeRate> Rates { get; set; } = new();
}

public class CnbExchangeRate
{
    // Include only properties that are needed for calculations.
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }
}
