namespace REST.Request.Models.Areas.Users;

/// <summary>
/// Request model for changing user password.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Current password for verification.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password to set.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
