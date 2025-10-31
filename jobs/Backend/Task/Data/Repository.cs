using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data;

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