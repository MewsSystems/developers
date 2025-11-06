using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.Queries.Users.GetUserById;

/// <summary>
/// Query to retrieve a user by their unique identifier.
/// </summary>
public record GetUserByIdQuery(int UserId) : IQuery<UserDetailDto?>;
