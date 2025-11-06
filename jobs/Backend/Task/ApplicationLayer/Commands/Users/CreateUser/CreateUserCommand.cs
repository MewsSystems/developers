using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Enums;

namespace ApplicationLayer.Commands.Users.CreateUser;

/// <summary>
/// Command to create a new user account.
/// </summary>
public record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role) : ICommand<Result<int>>;
