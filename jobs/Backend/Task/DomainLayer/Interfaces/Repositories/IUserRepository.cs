using DomainLayer.Aggregates.UserAggregate;

namespace DomainLayer.Interfaces.Repositories;

/// <summary>
/// Repository for managing user aggregates.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users.
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user with the given email exists.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user aggregate.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user aggregate.
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user aggregate.
    /// </summary>
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);
}
