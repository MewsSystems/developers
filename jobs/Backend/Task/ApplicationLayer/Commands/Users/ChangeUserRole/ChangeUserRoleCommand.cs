using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Enums;

namespace ApplicationLayer.Commands.Users.ChangeUserRole;

/// <summary>
/// Command to change a user's role.
/// </summary>
public record ChangeUserRoleCommand(
    int UserId,
    UserRole NewRole) : ICommand<Result>;
