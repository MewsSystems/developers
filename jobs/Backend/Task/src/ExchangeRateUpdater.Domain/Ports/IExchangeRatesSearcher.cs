using Domain.Entities;

namespace Domain.Ports;

public interface IExchangeRatesSearcher
{
    /// <summary>
    /// Get exchange rates for the given date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public Task<IEnumerable<ExchangeRate>>  GetExchangeRates(DateTime date);
}