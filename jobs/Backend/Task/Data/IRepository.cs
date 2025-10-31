using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data;

public interface IRepository<T> where T : ExchangeRateEntity
{
    Task AddExchangeRateAsync(ExchangeRateEntity exchangeRate);

    Task SaveChangesAsync();
}