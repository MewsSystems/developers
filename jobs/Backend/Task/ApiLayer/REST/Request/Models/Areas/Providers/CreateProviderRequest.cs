namespace REST.Request.Models.Areas.Providers;

/// <summary>
/// Request model for creating a new provider.
/// </summary>
public class CreateProviderRequest
{
    /// <summary>
    /// Provider name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Provider code (e.g., ECB, CNB, BNR).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Provider API URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Base currency ID for the provider.
    /// </summary>
    public int BaseCurrencyId { get; set; }

    /// <summary>
    /// Whether the provider requires authentication.
    /// </summary>
    public bool RequiresAuthentication { get; set; }

    /// <summary>
    /// API key vault reference (if authentication required).
    /// </summary>
    public string? ApiKeyVaultReference { get; set; }
}
