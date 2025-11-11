using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataLayer.Repositories;

public class SystemConfigurationRepository : Repository<SystemConfiguration>, ISystemConfigurationRepository
{
    public SystemConfigurationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SystemConfiguration?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Key == key, cancellationToken);
    }

    public async Task<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var config = await GetByKeyAsync(key, cancellationToken);
        if (config == null) return default;

        try
        {
            return config.DataType switch
            {
                "String" => (T)(object)config.Value,
                "Int" => (T)(object)int.Parse(config.Value),
                "Bool" => (T)(object)bool.Parse(config.Value),
                "DateTime" => (T)(object)DateTimeOffset.Parse(config.Value),
                "Decimal" => (T)(object)decimal.Parse(config.Value),
                _ => JsonSerializer.Deserialize<T>(config.Value) ?? default(T)
            };
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException($"Configuration value for key '{key}' has invalid format. Expected type: {config.DataType}, Value: '{config.Value}'", ex);
        }
        catch (InvalidCastException ex)
        {
            throw new InvalidOperationException($"Configuration value for key '{key}' cannot be cast to type {typeof(T).Name}. DataType: {config.DataType}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Configuration value for key '{key}' contains invalid JSON. Value: '{config.Value}'", ex);
        }
    }

    public async Task SetValueAsync<T>(string key, T value, int? modifiedBy = null, CancellationToken cancellationToken = default)
    {
        var config = await GetByKeyAsync(key, cancellationToken);
        var stringValue = value?.ToString() ?? string.Empty;
        var dataType = DetermineDataType<T>();

        if (config == null)
        {
            config = new SystemConfiguration
            {
                Key = key,
                Value = stringValue,
                DataType = dataType,
                Modified = DateTimeOffset.UtcNow,
                ModifiedBy = modifiedBy
            };
            await AddAsync(config, cancellationToken);
        }
        else
        {
            config.Value = stringValue;
            config.DataType = dataType;
            config.Modified = DateTimeOffset.UtcNow;
            config.ModifiedBy = modifiedBy;
            await UpdateAsync(config, cancellationToken);
        }
    }

    private static string DetermineDataType<T>()
    {
        var type = typeof(T);
        if (type == typeof(string)) return "String";
        if (type == typeof(int) || type == typeof(long)) return "Int";
        if (type == typeof(bool)) return "Bool";
        if (type == typeof(DateTime) || type == typeof(DateTimeOffset)) return "DateTime";
        if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)) return "Decimal";
        return "String";
    }
}
