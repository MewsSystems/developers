using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

public interface IExchangeRateClient<T> 
{
    /// <summary>
    /// Retrieves ExchangeRate information from a Response <see cref="T"/> via an external data source.
    /// </summary>
    /// <returns>Response <see cref="T"/> from the external data source </returns>
    Task<T> RetrieveExchangeRatesAsync();
}