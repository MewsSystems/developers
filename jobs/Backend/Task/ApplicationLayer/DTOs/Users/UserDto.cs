namespace ApplicationLayer.DTOs.Users;

/// <summary>
/// DTO for user information.
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset? LastLogin { get; set; }
    public DateTimeOffset Created { get; set; }
}
