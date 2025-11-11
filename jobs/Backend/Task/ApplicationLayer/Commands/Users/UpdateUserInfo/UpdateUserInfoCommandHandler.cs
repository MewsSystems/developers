using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.UpdateUserInfo;

public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserInfoCommandHandler> _logger;

    public UpdateUserInfoCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserInfoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateUserInfoCommand request,
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

            // Check if email is changing and if new email already exists
            var emailToUpdate = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !user.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _unitOfWork.Users
                    .GetByEmailAsync(request.Email, cancellationToken);

                if (existingUser != null && existingUser.Id != user.Id)
                {
                    _logger.LogWarning(
                        "Attempt to change email to existing email: {Email}",
                        request.Email);
                    return Result.Failure($"Email '{request.Email}' is already in use.");
                }
            }
            else if (string.IsNullOrWhiteSpace(request.Email))
            {
                // If no email provided, keep the existing email
                emailToUpdate = user.Email;
            }

            // Update user info
            user.UpdateInfo(request.FirstName, request.LastName, emailToUpdate!);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Updated user info for {UserId}: {Email}",
                user.Id,
                user.Email);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error updating user {UserId}", request.UserId);
            return Result.Failure(ex.Message);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", request.UserId);
            return Result.Failure($"Failed to update user: {ex.Message}");
        }
    }
}
