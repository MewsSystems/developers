namespace ApplicationLayer.DTOs.Authentication;

/// <summary>
/// DTO representing the result of an authentication operation.
/// </summary>
public class AuthenticationResultDto
{
    /// <summary>
    /// User ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// User email address.
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
    /// User's role (Admin, Consumer).
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// JWT access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token for obtaining new access tokens.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time (Unix timestamp).
    /// </summary>
    public long ExpiresAt { get; set; }
}
