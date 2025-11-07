namespace REST.Response.Models.Areas.Users;

/// <summary>
/// API response model for basic user information.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// User unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User role (e.g., "Admin", "Consumer").
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// User full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Indicates if user has admin role.
    /// </summary>
    public bool IsAdmin => Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;
}
