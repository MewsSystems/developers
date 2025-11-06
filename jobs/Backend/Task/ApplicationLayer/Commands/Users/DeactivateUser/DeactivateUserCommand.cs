using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Users.DeactivateUser;

/// <summary>
/// Command to deactivate a user account.
/// </summary>
public record DeactivateUserCommand(int UserId) : ICommand<Result>;
