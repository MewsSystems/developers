using Domain.Model;

namespace Application.Abstractions;

public interface IExchangeRatesService
{
    /// <summary>
    /// Gets the latest exchange rates using the CNB API client. Cashed the data with 30 minutes expiration.
    /// </summary>
    /// <returns>List of all available exchange rates.</returns>
    Task<IEnumerable<ExchangeRateDetails>> GetCashedExchangeRatesAsync();
}