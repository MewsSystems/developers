namespace REST.Request.Models.Areas.Providers;

/// <summary>
/// Request model for updating provider configuration.
/// </summary>
public class UpdateProviderConfigurationRequest
{
    /// <summary>
    /// Updated provider name (optional).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Updated API URL (optional).
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Updated authentication requirement (optional).
    /// </summary>
    public bool? RequiresAuthentication { get; set; }

    /// <summary>
    /// Updated API key vault reference (optional).
    /// </summary>
    public string? ApiKeyVaultReference { get; set; }
}
