using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.ChangeUserPassword;

public class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ChangeUserPasswordCommandHandler> _logger;

    public ChangeUserPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<ChangeUserPasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result> Handle(
        ChangeUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get user
            var user = await _unitOfWork.Users
                .GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", request.UserId);
                throw new NotFoundException("User", request.UserId);
            }

            // Verify current password
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarning("Invalid current password for user {UserId}", request.UserId);
                return Result.Failure("Current password is incorrect.");
            }

            // Hash new password
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            // Change password
            user.ChangePassword(newPasswordHash);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Password changed for user {UserId}", user.Id);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error changing password for user {UserId}", request.UserId);
            return Result.Failure(ex.Message);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", request.UserId);
            return Result.Failure($"Failed to change password: {ex.Message}");
        }
    }
}
