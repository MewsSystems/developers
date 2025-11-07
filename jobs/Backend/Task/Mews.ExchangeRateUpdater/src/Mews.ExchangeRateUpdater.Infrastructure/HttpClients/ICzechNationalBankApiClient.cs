using Mews.ExchangeRateUpdater.Domain.Entities;

namespace Mews.ExchangeRateUpdater.Infrastructure.HttpClients;

/// <summary>
/// Exchange rates application service definition.
/// </summary>
public interface ICzechNationalBankApiClient
{
    /// <summary>
    /// Gets a collection of today's exchange rates.
    /// </summary>
    /// <returns>List of <see cref="CNBExchangeRates"/></returns>
    Task<CNBExchangeRates> GetTodayExchangeRatesAsync();
}
