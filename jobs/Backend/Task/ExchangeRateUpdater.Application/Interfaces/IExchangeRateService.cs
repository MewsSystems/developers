using ExchangeRateUpdater.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Interfaces;

/// <summary>
/// Defines a service for retrieving exchange rates from external sources.
/// </summary>
public interface IExchangeRateService
{
    /// <summary>
    /// Asynchronously retrieves exchange rates for a given date and an optional list of currencies.
    /// </summary>
    /// <param name="date">The date for which exchange rates should be retrieved.</param>
    /// <param name="currencies">
    /// A collection of currency codes (ISO 4217) to filter the results.
    /// If <c>null</c> or empty, returns exchange rates for all supported currencies.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="ExchangeRateResponse"/> object with exchange rates for the given date.
    /// </returns>
    Task<ExchangeRateResponse> GetExchangeRatesAsync(
        DateTime date,
        IEnumerable<string>? currencies,
        CancellationToken cancellationToken);
}
