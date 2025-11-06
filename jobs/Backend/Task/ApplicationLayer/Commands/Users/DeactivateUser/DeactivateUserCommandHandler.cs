using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Enums;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.DeactivateUser;

public class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateUserCommandHandler> _logger;

    public DeactivateUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeactivateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeactivateUserCommand request,
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

            // Check if this is the last active admin
            if (user.Role == UserRole.Admin && user.IsActive)
            {
                var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);
                var adminCount = allUsers.Count(u => u.Role == UserRole.Admin && u.IsActive);

                if (adminCount <= 1)
                {
                    _logger.LogWarning("Attempt to deactivate the last active admin user {UserId}", request.UserId);
                    return Result.Failure("Cannot deactivate the last active admin user.");
                }
            }

            // Deactivate user
            user.Deactivate();

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deactivated user {UserId}", user.Id);

            return Result.Success();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user {UserId}", request.UserId);
            return Result.Failure($"Failed to deactivate user: {ex.Message}");
        }
    }
}
