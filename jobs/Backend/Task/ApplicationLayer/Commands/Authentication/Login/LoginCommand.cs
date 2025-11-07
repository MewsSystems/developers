using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Authentication;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.Authentication.Login;

/// <summary>
/// Command to authenticate a user and generate tokens.
/// </summary>
/// <param name="Email">User email address</param>
/// <param name="Password">User password</param>
public record LoginCommand(string Email, string Password) : ICommand<Result<AuthenticationResultDto>>;
