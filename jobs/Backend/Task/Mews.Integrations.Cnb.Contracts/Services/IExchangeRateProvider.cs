using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Contracts.Models;

namespace Mews.Integrations.Cnb.Contracts.Services;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Retrieves exchange rates for the specified currencies and date.
    /// </summary>
    /// <param name="currencies">Currency codes</param>
    /// <param name="date">Target date, if date is in future, the latest available exchange rates are returned</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        DateTimeOffset date,
        CancellationToken cancellationToken);
}
