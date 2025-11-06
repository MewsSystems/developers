using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.GetCurrentUser;

public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;

    public GetCurrentUserQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCurrentUserQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserDetailDto?> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting current user: {UserId}", request.CurrentUserId);

        var user = await _unitOfWork.Users
            .GetByIdAsync(request.CurrentUserId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Current user {UserId} not found", request.CurrentUserId);
            return null;
        }

        return new UserDetailDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            LastLogin = user.LastLogin,
            Created = user.Created,
            Modified = user.Modified
        };
    }
}
