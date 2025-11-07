using ApplicationLayer.DTOs.Authentication;
using REST.Response.Models.Areas.Authentication;

namespace REST.Response.Converters;

/// <summary>
/// Converter extensions for authentication DTOs.
/// </summary>
public static class AuthenticationConverters
{
    /// <summary>
    /// Converts an AuthenticationResultDto to an AuthenticationResponse.
    /// </summary>
    public static AuthenticationResponse ToResponse(this AuthenticationResultDto dto)
    {
        return new AuthenticationResponse
        {
            UserId = dto.UserId,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            AccessToken = dto.AccessToken,
            RefreshToken = dto.RefreshToken,
            ExpiresAt = dto.ExpiresAt
        };
    }
}
