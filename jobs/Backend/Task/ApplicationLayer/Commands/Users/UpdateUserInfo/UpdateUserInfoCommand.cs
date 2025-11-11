using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Users.UpdateUserInfo;

/// <summary>
/// Command to update user profile information.
/// </summary>
public record UpdateUserInfoCommand(
    int UserId,
    string FirstName,
    string LastName,
    string? Email = null) : ICommand<Result>;
