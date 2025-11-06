using ApplicationLayer.Common.Abstractions;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _unitOfWork.Users
                .GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser != null)
            {
                _logger.LogWarning("Attempt to create user with existing email: {Email}", request.Email);
                return Result.Failure<int>($"User with email '{request.Email}' already exists.");
            }

            // Hash the password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create user aggregate
            var user = User.Create(
                request.Email,
                passwordHash,
                request.FirstName,
                request.LastName,
                request.Role);

            // Add to repository
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Query back to get the generated ID
            var savedUser = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
            var userId = savedUser?.Id ?? 0;

            _logger.LogInformation(
                "Created user {Email} with ID {UserId} and role {Role}",
                request.Email,
                userId,
                request.Role);

            return Result.Success(userId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating user: {Email}", request.Email);
            return Result.Failure<int>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", request.Email);
            return Result.Failure<int>($"Failed to create user: {ex.Message}");
        }
    }
}
