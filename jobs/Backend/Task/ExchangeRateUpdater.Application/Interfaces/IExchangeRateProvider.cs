using ExchangeRateUpdater.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Interfaces;

/// <summary>
/// Defines a contract for retrieving exchange rates from an external data source.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Asynchronously fetches exchange rates for the specified date.
    /// </summary>
    /// <param name="date">The date for which exchange rates should be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of <see cref="ExchangeRate"/> objects for the given date.
    /// </returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        DateTime date,
        CancellationToken cancellationToken = default);
}
