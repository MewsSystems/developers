using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Users.ChangeUserPassword;

/// <summary>
/// Command to change a user's password.
/// </summary>
public record ChangeUserPasswordCommand(
    int UserId,
    string CurrentPassword,
    string NewPassword) : ICommand<Result>;
