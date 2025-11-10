namespace REST.Request.Models.Areas.Users;

/// <summary>
/// Request model for updating user information.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// User first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User email address (optional - if not provided, email remains unchanged).
    /// </summary>
    public string? Email { get; set; }
}
