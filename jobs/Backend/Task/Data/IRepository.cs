using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data;

/// <summary>
///     Defines repository operations for persisting exchange rate data.
///     Provides abstraction for database operations on exchange rate entities.
/// </summary>
/// <typeparam name="T">The entity type, constrained to <see cref="ExchangeRateEntity" />.</typeparam>
public interface IRepository<T> where T : ExchangeRateEntity
{
    Task AddExchangeRateAsync(ExchangeRateEntity exchangeRate);

    Task SaveChangesAsync();
}