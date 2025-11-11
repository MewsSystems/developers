namespace REST.Response.Models.Areas.Authentication;

/// <summary>
/// Response model for authentication operations (login/register).
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// User's unique identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's role (Admin or Consumer).
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// JWT access token for authentication.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token for obtaining new access tokens.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Unix timestamp indicating when the access token expires.
    /// </summary>
    public long ExpiresAt { get; set; }
}
