namespace InfrastructureLayer.Authentication;

/// <summary>
/// JWT configuration settings mapped from appsettings.json.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Authentication:Jwt";

    /// <summary>
    /// Secret key used for signing tokens.
    /// IMPORTANT: Must be at least 32 characters (256 bits) for HS256.
    /// </summary>
    public string SecretKey { get; init; } = string.Empty;

    /// <summary>
    /// Token issuer (who created the token).
    /// </summary>
    public string Issuer { get; init; } = string.Empty;

    /// <summary>
    /// Token audience (who the token is intended for).
    /// </summary>
    public string Audience { get; init; } = string.Empty;

    /// <summary>
    /// Access token expiration time in minutes.
    /// </summary>
    public int ExpirationMinutes { get; init; } = 60;

    /// <summary>
    /// Refresh token expiration time in days.
    /// </summary>
    public int RefreshTokenExpirationDays { get; init; } = 7;
}
