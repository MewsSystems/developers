using System.ComponentModel.DataAnnotations;

namespace REST.Request.Models.Areas.Providers;

/// <summary>
/// Request model for rescheduling a provider's job.
/// </summary>
public class RescheduleProviderRequest
{
    /// <summary>
    /// Time when the provider should fetch rates (HH:mm format, e.g., "14:30").
    /// </summary>
    [Required(ErrorMessage = "UpdateTime is required")]
    [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "UpdateTime must be in HH:mm format (e.g., 14:30)")]
    public string UpdateTime { get; set; } = string.Empty;

    /// <summary>
    /// Timezone for the update time (e.g., "CET", "UTC", "EET").
    /// </summary>
    [Required(ErrorMessage = "TimeZone is required")]
    public string TimeZone { get; set; } = string.Empty;
}
