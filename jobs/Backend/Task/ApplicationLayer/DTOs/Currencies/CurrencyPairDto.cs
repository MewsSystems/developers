namespace ApplicationLayer.DTOs.Currencies;

/// <summary>
/// DTO for currency pair availability information.
/// </summary>
public class CurrencyPairDto
{
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int ProviderCount { get; set; }
    public List<string> AvailableProviders { get; set; } = new();
    public DateOnly? LatestRateDate { get; set; }
    public decimal? LatestRate { get; set; }
}
