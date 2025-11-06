using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Users.DeleteUser;

/// <summary>
/// Command to permanently delete a user account.
/// </summary>
public record DeleteUserCommand(int UserId) : ICommand<Result>;
