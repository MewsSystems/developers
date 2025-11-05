using DataLayer.Entities;

namespace DataLayer.Repositories;

public interface ISystemConfigurationRepository : IRepository<SystemConfiguration>
{
    Task<SystemConfiguration?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetValueAsync<T>(string key, T value, int? modifiedBy = null, CancellationToken cancellationToken = default);
}
