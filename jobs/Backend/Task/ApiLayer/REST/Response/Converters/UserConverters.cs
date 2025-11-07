using ApplicationLayer.DTOs.Users;
using REST.Response.Models.Areas.Users;

namespace REST.Response.Converters;

/// <summary>
/// Converters for transforming User DTOs to API response models.
/// </summary>
public static class UserConverters
{
    /// <summary>
    /// Converts UserDto to UserResponse.
    /// </summary>
    public static UserResponse ToResponse(this UserDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
        };
    }

    /// <summary>
    /// Converts UserDetailDto to UserResponse.
    /// </summary>
    public static UserResponse ToResponse(this UserDetailDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
        };
    }
}
