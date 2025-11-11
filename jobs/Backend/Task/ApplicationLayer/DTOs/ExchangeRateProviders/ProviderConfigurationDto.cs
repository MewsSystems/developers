namespace ApplicationLayer.DTOs.ExchangeRateProviders;

/// <summary>
/// DTO for provider configuration settings.
/// </summary>
public class ProviderConfigurationDto
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}
