using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Enums;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.ChangeUserRole;

public class ChangeUserRoleCommandHandler : ICommandHandler<ChangeUserRoleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangeUserRoleCommandHandler> _logger;

    public ChangeUserRoleCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ChangeUserRoleCommandHandler> _logger)
    {
        _unitOfWork = unitOfWork;
        this._logger = _logger;
    }

    public async Task<Result> Handle(
        ChangeUserRoleCommand request,
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

            // Check if changing the last admin to a different role
            if (user.Role == UserRole.Admin && request.NewRole != UserRole.Admin)
            {
                var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);
                var adminCount = allUsers.Count(u => u.Role == UserRole.Admin);

                if (adminCount <= 1)
                {
                    _logger.LogWarning("Attempt to change role of the last admin user {UserId}", request.UserId);
                    return Result.Failure("Cannot change the role of the last admin user.");
                }
            }

            // Change role
            user.ChangeRole(request.NewRole);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Changed role for user {UserId} to {Role}",
                user.Id,
                request.NewRole);

            return Result.Success();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing role for user {UserId}", request.UserId);
            return Result.Failure($"Failed to change user role: {ex.Message}");
        }
    }
}
