using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Enums;

namespace ApplicationLayer.Queries.Users.GetUsersByRole;

/// <summary>
/// Query to get users filtered by role with optional pagination.
/// </summary>
public record GetUsersByRoleQuery(
    UserRole Role,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<UserDto>>;
