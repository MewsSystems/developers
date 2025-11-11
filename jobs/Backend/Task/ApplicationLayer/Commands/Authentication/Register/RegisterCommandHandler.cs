using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Authentication;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Common;
using DomainLayer.Enums;
using DomainLayer.Interfaces.Repositories;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Authentication.Register;

/// <summary>
/// Handler for RegisterCommand.
/// Creates a new user account and generates JWT tokens.
/// </summary>
public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<AuthenticationResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<AuthenticationResultDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing registration request for email: {Email}", request.Email);

        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: User already exists with email {Email}", request.Email);
            return Result<AuthenticationResultDto>.Failure("A user with this email address already exists.");
        }

        // Hash password
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // Determine role (default to Consumer if not specified)
        var role = request.Role ?? UserRole.Consumer;

        // Create user
        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            role);

        // Save user
        await _userRepository.AddAsync(user, cancellationToken);

        // Generate tokens
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // Calculate expiration time
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds();

        _logger.LogInformation("Registration successful for user {Email} (ID: {UserId})", user.Email, user.Id);

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
