using ApplicationLayer.DTOs.Authentication;
using DomainLayer.Common;
using gRPC.Protos.Authentication;

namespace gRPC.Mappers;

/// <summary>
/// Mappers for authentication-related DTOs and proto messages.
/// </summary>
public static class AuthenticationMappers
{
    // ============================================================
    // LOGIN RESPONSE
    // ============================================================

    public static LoginResponse ToProtoLoginResponse(Result<AuthenticationResultDto> result)
    {
        if (result.IsSuccess && result.Value != null)
        {
            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Data = ToProtoAuthenticationData(result.Value)
            };
        }

        return new LoginResponse
        {
            Success = false,
            Message = result.Error ?? "Login failed",
            Error = result.Error != null ? CommonMappers.ToProtoError("AUTH_ERROR", result.Error) : null
        };
    }

    // ============================================================
    // AUTHENTICATION DATA
    // ============================================================

    private static AuthenticationData ToProtoAuthenticationData(AuthenticationResultDto dto)
    {
        // Calculate seconds until expiration
        var expiresIn = dto.ExpiresAt - DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return new AuthenticationData
        {
            AccessToken = dto.AccessToken,
            TokenType = "Bearer",
            ExpiresInSeconds = expiresIn,
            User = new UserInfo
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FullName = $"{dto.FirstName} {dto.LastName}".Trim(),
                Role = dto.Role
            }
        };
    }
}
