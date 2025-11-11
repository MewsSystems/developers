using ApplicationLayer.Common.Abstractions;

namespace ApplicationLayer.Queries.Users.CheckEmailExists;

/// <summary>
/// Query to check if a user with the given email exists.
/// </summary>
public record CheckEmailExistsQuery(string Email) : IQuery<bool>;
