using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Enums;

namespace ApplicationLayer.Queries.Users.GetAllUsers;

/// <summary>
/// Query to get all users with optional pagination and filtering.
/// </summary>
public record GetAllUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    UserRole? RoleFilter = null) : IQuery<PagedResult<UserDto>>;
