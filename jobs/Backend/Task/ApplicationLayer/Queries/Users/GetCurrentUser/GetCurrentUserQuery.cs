using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.Queries.Users.GetCurrentUser;

/// <summary>
/// Query to retrieve the currently authenticated user's information.
/// </summary>
public record GetCurrentUserQuery(int CurrentUserId) : IQuery<UserDetailDto?>;
