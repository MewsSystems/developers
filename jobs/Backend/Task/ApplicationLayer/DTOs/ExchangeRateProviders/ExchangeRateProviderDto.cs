namespace ApplicationLayer.DTOs.ExchangeRateProviders;

/// <summary>
/// DTO for basic exchange rate provider information.
/// </summary>
public class ExchangeRateProviderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public DateTimeOffset Created { get; set; }
}
