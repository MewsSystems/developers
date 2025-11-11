namespace REST.Request.Models.Areas.Users;

/// <summary>
/// Request model for changing user role.
/// </summary>
public class ChangeRoleRequest
{
    /// <summary>
    /// New role to assign (Consumer or Admin).
    /// </summary>
    public string NewRole { get; set; } = string.Empty;
}
