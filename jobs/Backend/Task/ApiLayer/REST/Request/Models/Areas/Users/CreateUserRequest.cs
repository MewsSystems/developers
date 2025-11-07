namespace REST.Request.Models.Areas.Users;

/// <summary>
/// Request model for creating a user.
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User role (Consumer or Admin).
    /// </summary>
    public string Role { get; set; } = "Consumer";
}
