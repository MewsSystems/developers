namespace ExchangeRateUpdater.Application.Settings;

/// <summary>
/// Represents API configuration settings.
/// </summary>
public class ApiSettings
{
    /// <summary>
    /// Gets or sets the base URL of the external API.
    /// </summary>
    /// <value>
    /// The base URL as a string. Defaults to an empty string if not set.
    /// </value>
    public string BaseUrl { get; set; } = string.Empty;
}
