using Newtonsoft.Json;

namespace ExchangeRateUpdaterApi.Dtos.Response;

/// <summary>
/// Dto containing information about an ExchangeRate pair
/// </summary>
public class ExchangeRateResultDto
{
    /// <summary>
    /// SourceCurrency information
    /// </summary>
    [JsonProperty("sourceCurrency")] 
    public CurrencyDto SourceCurrency { get; set; }
    /// <summary>
    /// TargetCurrency information
    /// </summary>
    [JsonProperty("targetCurrency")]
    public CurrencyDto TargetCurrency { get; set; }
    /// <summary>
    /// Exchange Rate value between source and target currencies
    /// </summary>
    [JsonProperty("value")] 
    public decimal Value { get; set; }
}