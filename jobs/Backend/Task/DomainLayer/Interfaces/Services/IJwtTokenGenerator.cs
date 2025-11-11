using DomainLayer.Aggregates.UserAggregate;

namespace DomainLayer.Interfaces.Services;

/// <summary>
/// Service for generating JWT tokens.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates an access token for the user.
    /// </summary>
    /// <param name="user">The user to generate the token for</param>
    /// <returns>JWT access token</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>Refresh token string</returns>
    string GenerateRefreshToken();
}
