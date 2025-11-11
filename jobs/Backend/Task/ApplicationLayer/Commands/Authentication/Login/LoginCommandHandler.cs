using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Authentication;
using DomainLayer.Common;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Authentication.Login;

/// <summary>
/// Handler for LoginCommand.
/// Authenticates the user and generates JWT tokens.
/// </summary>
public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<AuthenticationResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing login request for email: {Email}", request.Email);

        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found with email {Email}", request.Email);
            return Result<AuthenticationResultDto>.Failure("Invalid email or password.");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password for email {Email}", request.Email);
            return Result<AuthenticationResultDto>.Failure("Invalid email or password.");
        }

        // Generate tokens
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // Calculate expiration time
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds();

        // Record login
        user.RecordLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Login successful for user {Email} (ID: {UserId})", user.Email, user.Id);

        var result = new AuthenticationResultDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };

        return Result<AuthenticationResultDto>.Success(result);
    }
}
