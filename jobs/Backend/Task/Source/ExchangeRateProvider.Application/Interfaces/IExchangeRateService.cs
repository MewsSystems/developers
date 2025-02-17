namespace ExchangeRateProvider.Application.Interfaces;

using Domain.DTOs;

/// <summary>
///     Service interface for retrieving exchange rates.
/// </summary>
public interface IExchangeRateService
{
    /// <summary>
    ///     Retrieves exchange rates for the specified currencies.
    /// </summary>
    /// <param name="requestedCurrencies">A collection of currency codes in ISO 4217 format.</param>
    /// <returns>An object containing the valid exchange rates and unresolved currencies.</returns>
    Task<ExchangeRateResult> GetExchangeRatesAsync(IEnumerable<string> requestedCurrencies);
}
