using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data;

/// <summary>
///     Implements repository pattern for persisting exchange rate entities to the database.
///     Provides data access operations using Entity Framework Core.
/// </summary>
/// <typeparam name="T">The entity type, constrained to <see cref="ExchangeRateEntity" />.</typeparam>
public class Repository<T> : IRepository<T> where T : ExchangeRateEntity
{
    private readonly ExchangeRateDbContext _dbContext;

    public Repository(ExchangeRateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddExchangeRateAsync(ExchangeRateEntity exchangeRate)
    {
        await _dbContext.ExchangeRates.AddAsync(exchangeRate);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}