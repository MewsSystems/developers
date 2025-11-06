using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Getting all users with PageNumber: {PageNumber}, PageSize: {PageSize}, SearchTerm: {SearchTerm}, RoleFilter: {RoleFilter}",
            request.PageNumber, request.PageSize, request.SearchTerm, request.RoleFilter);

        // Get all users
        var allUsers = await _unitOfWork.Users.GetAllAsync(cancellationToken);

        // Apply filtering
        var filteredUsers = allUsers.AsEnumerable();

        if (request.RoleFilter.HasValue)
        {
            filteredUsers = filteredUsers.Where(u => u.Role == request.RoleFilter.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            filteredUsers = filteredUsers.Where(u =>
                u.Email.ToLowerInvariant().Contains(searchTerm) ||
                u.FirstName.ToLowerInvariant().Contains(searchTerm) ||
                u.LastName.ToLowerInvariant().Contains(searchTerm) ||
                u.FullName.ToLowerInvariant().Contains(searchTerm));
        }

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

        _logger.LogDebug("Retrieved {Count} users out of {TotalCount} total", dtos.Count, totalCount);

        return PagedResult<UserDto>.Create(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
