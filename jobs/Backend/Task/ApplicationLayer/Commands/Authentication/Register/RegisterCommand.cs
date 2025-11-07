using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Authentication;
using DomainLayer.Common;
using DomainLayer.Enums;

namespace ApplicationLayer.Commands.Authentication.Register;

/// <summary>
/// Command to register a new user.
/// </summary>
/// <param name="Email">User email address</param>
/// <param name="Password">User password</param>
/// <param name="FirstName">User's first name</param>
/// <param name="LastName">User's last name</param>
/// <param name="Role">User role (defaults to Consumer if not specified)</param>
public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole? Role = null) : ICommand<Result<AuthenticationResultDto>>;
