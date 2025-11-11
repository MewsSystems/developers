namespace REST.Response.Models.Common;

/// <summary>
/// Provider information nested object for API responses.
/// </summary>
public class ProviderInfo
{
    /// <summary>
    /// Provider unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Provider code (e.g., "ECB", "CNB", "BNR").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Provider name (e.g., "European Central Bank").
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
