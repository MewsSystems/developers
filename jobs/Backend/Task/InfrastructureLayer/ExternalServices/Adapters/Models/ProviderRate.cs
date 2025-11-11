namespace InfrastructureLayer.ExternalServices.Adapters.Models;

/// <summary>
/// Represents a single exchange rate from a provider.
/// Simplified model used by adapters.
/// </summary>
public class ProviderRate
{
    public string SourceCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public decimal Multiplier { get; set; }
    public DateOnly ValidDate { get; set; }
}
