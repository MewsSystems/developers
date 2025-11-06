using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.GetUsersByRole;

public class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUsersByRoleQueryHandler> _logger;

    public GetUsersByRoleQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUsersByRoleQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<UserDto>> Handle(
        GetUsersByRoleQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Getting users by role: {Role}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.Role, request.PageNumber, request.PageSize);

        // Get all users
        var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);

        // Apply filtering
        var filteredUsers = allUsers
            .Where(u => u.Role == request.Role);

        var totalCount = filteredUsers.Count();

        // Apply pagination
        var pagedUsers = filteredUsers
            .OrderBy(u => u.Email)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Map to DTOs
        var dtos = pagedUsers.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            FullName = u.FullName,
            Role = u.Role.ToString()
        }).ToList();

        _logger.LogDebug("Retrieved {Count} users with role {Role} out of {TotalCount} total",
            dtos.Count, request.Role, totalCount);

        return PagedResult<UserDto>.Create(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
