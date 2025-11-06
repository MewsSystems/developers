using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Users.ActivateUser;

/// <summary>
/// Command to activate a user account.
/// </summary>
public record ActivateUserCommand(int UserId) : ICommand<Result>;
