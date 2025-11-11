namespace ApplicationLayer.DTOs.ExchangeRateProviders;

/// <summary>
/// DTO for detailed exchange rate provider information including configurations.
/// </summary>
public class ExchangeRateProviderDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public bool RequiresAuthentication { get; set; }
    public string? ApiKeyVaultReference { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public List<ProviderConfigurationDto> Configurations { get; set; } = new();
}
