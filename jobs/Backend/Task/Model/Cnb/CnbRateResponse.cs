using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Model.Cnb;

public class CnbRate
{
    [JsonPropertyName("validFor")]
    public DateTime ValidFor { get; set; }

    [JsonPropertyName("order")]
    public int Order {  get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set;}

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set;}

    [JsonPropertyName("rate")]
    public decimal RateVal { get; set;}
}

public class RateResponse
{
    public IEnumerable<CnbRate> Rates { get; set; }
}