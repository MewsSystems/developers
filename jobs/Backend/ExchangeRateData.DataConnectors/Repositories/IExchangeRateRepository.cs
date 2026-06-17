using ExchangeRateData.DataConnectors.Models;

namespace ExchangeRateData.DataConnectors.Repositories;

public interface IExchangeRateRepository
{
    /// <summary>
    /// Returns list of exchange rate data valid to submitted date and of given currencies 
    /// </summary>
    /// <param name="submittedDate">Date for which exchange rates are sought</param>
    /// /// <param name="currencies">Codes of currencies given by user</param>
    /// <returns>ExchangeRate</returns>
    Task<IEnumerable<ExchangeRate>>? GetExchangeRatesByCurrencyAndDateTaskAsync(string submittedDate, string[] currencies);
}
