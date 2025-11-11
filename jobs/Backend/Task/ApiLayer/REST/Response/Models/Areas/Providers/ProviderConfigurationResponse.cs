namespace REST.Response.Models.Areas.Providers;

/// <summary>
/// API response model for provider configuration settings.
/// </summary>
public class ProviderConfigurationResponse
{
    /// <summary>
    /// Configuration setting unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Provider ID this configuration belongs to.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Configuration key name.
    /// </summary>
    public string SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// Configuration value.
    /// </summary>
    public string SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// Description of what this setting controls.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// When this configuration was created.
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// When this configuration was last modified.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }
}
