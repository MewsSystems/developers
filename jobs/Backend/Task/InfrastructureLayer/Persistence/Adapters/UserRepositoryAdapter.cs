using DataLayer;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Interfaces.Repositories;

namespace InfrastructureLayer.Persistence.Adapters;

/// <summary>
/// Adapts DataLayer user repository to DomainLayer interface.
/// Uses the User.Reconstruct factory method for proper aggregate hydration.
/// </summary>
public class UserRepositoryAdapter : IUserRepository
{
    private readonly IUnitOfWork _dataLayerUnitOfWork;

    public UserRepositoryAdapter(IUnitOfWork dataLayerUnitOfWork)
    {
        _dataLayerUnitOfWork = dataLayerUnitOfWork;
    }

    public async Task<DomainLayer.Aggregates.UserAggregate.User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Users.GetByIdAsync(id, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<DomainLayer.Aggregates.UserAggregate.User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Users.GetByEmailAsync(email, cancellationToken);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<DomainLayer.Aggregates.UserAggregate.User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dataLayerUnitOfWork.Users.GetAllAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dataLayerUnitOfWork.Users.EmailExistsAsync(email, cancellationToken);
    }

    public async Task AddAsync(DomainLayer.Aggregates.UserAggregate.User user, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(user);
        await _dataLayerUnitOfWork.Users.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(DomainLayer.Aggregates.UserAggregate.User user, CancellationToken cancellationToken = default)
    {
        // Get the tracked entity and update its properties instead of creating a new entity
        // This avoids EF Core tracking conflicts
        var existingEntity = await _dataLayerUnitOfWork.Users.GetByIdAsync(user.Id, cancellationToken);
        if (existingEntity != null)
        {
            existingEntity.Email = user.Email;
            existingEntity.FirstName = user.FirstName;
            existingEntity.LastName = user.LastName;
            existingEntity.Role = user.Role.ToString();
            existingEntity.PasswordHash = user.PasswordHash;

            await _dataLayerUnitOfWork.Users.UpdateAsync(existingEntity, cancellationToken);
        }
    }

    public async Task DeleteAsync(DomainLayer.Aggregates.UserAggregate.User user, CancellationToken cancellationToken = default)
    {
        var entity = await _dataLayerUnitOfWork.Users.GetByIdAsync(user.Id, cancellationToken);
        if (entity != null)
        {
            await _dataLayerUnitOfWork.Users.DeleteAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Maps DataLayer entity to Domain aggregate.
    /// Uses the Reconstruct factory method for proper aggregate hydration.
    /// </summary>
    private static DomainLayer.Aggregates.UserAggregate.User MapToDomain(DataLayer.Entities.User entity)
    {
        // Parse UserRole from string
        if (!Enum.TryParse<DomainLayer.Enums.UserRole>(entity.Role, true, out var role))
        {
            // Default to Consumer if parsing fails
            role = DomainLayer.Enums.UserRole.Consumer;
        }

        return DomainLayer.Aggregates.UserAggregate.User.Reconstruct(
            id: entity.Id,
            email: entity.Email,
            passwordHash: entity.PasswordHash,
            firstName: entity.FirstName,
            lastName: entity.LastName,
            role: role);
    }

    private static DataLayer.Entities.User MapToEntity(DomainLayer.Aggregates.UserAggregate.User domain)
    {
        return new DataLayer.Entities.User
        {
            Id = domain.Id,
            Email = domain.Email,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            Role = domain.Role.ToString(),
            PasswordHash = domain.PasswordHash
        };
    }
}
