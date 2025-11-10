using ApplicationLayer.DTOs.Users;
using SOAP.Models.Users;

namespace SOAP.Converters;

/// <summary>
/// Converter extensions for user DTOs to SOAP models.
/// </summary>
public static class UserSoapConverters
{
    /// <summary>
    /// Converts a UserDto to SOAP model.
    /// </summary>
    public static UserSoap ToSoap(this UserDto dto)
    {
        return new UserSoap
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = dto.FullName,
            Role = dto.Role,
            IsActive = true, // Default: users are active
            CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") // ISO 8601 format
        };
    }

    /// <summary>
    /// Converts a UserDetailDto to SOAP model.
    /// </summary>
    public static UserSoap ToSoap(this UserDetailDto dto)
    {
        return new UserSoap
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = dto.FullName,
            Role = dto.Role,
            IsActive = true, // Default: users are active
            CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") // ISO 8601 format
        };
    }

    /// <summary>
    /// Converts a collection of UserDto to SOAP models.
    /// </summary>
    public static UserSoap[] ToSoap(this IEnumerable<UserDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }

    /// <summary>
    /// Converts a collection of UserDetailDto to SOAP models.
    /// </summary>
    public static UserSoap[] ToSoap(this IEnumerable<UserDetailDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }
}
