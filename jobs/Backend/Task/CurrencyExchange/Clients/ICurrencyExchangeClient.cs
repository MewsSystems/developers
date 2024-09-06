using CurrencyExchange.Model;

namespace CurrencyExchange.Clients;

/// <summary>
/// Defines methods for obtaining daily exchange rates from external source.
/// </summary>
public interface ICurrencyExchangeClient
{
    /// <summary>
    /// Returns <see cref="DailyRatesResponse"/> with rates for the current date.
    /// </summary>
    /// <remarks> Could be enhanced to accept date as a parameter. CNB REST API should be able to return rates for different dates.</remarks>
    Task<DailyRatesResponse> GetDailyRates(CancellationToken cancellationToken);
}  