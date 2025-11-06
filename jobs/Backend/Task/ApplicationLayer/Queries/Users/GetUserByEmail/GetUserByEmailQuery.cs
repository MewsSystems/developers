using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;

namespace ApplicationLayer.Queries.Users.GetUserByEmail;

/// <summary>
/// Query to retrieve a user by their email address.
/// </summary>
public record GetUserByEmailQuery(string Email) : IQuery<UserDto?>;
