using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.Common.Exceptions;
using DomainLayer.Common;
using DomainLayer.Enums;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Users.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request,
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

            // Check if this is the last admin
            if (user.Role == UserRole.Admin)
            {
                var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);
                var adminCount = allUsers.Count(u => u.Role == UserRole.Admin);

                if (adminCount <= 1)
                {
                    _logger.LogWarning("Attempt to delete the last admin user {UserId}", request.UserId);
                    return Result.Failure("Cannot delete the last admin user.");
                }
            }

            // Delete user
            await _unitOfWork.Users.DeleteAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted user {UserId} ({Email})", user.Id, user.Email);

            return Result.Success();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", request.UserId);
            return Result.Failure($"Failed to delete user: {ex.Message}");
        }
    }
}
