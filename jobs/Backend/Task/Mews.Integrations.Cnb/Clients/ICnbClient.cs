using System;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Models;

namespace Mews.Integrations.Cnb.Clients;

public interface ICnbClient
{
    /// <summary>
    /// Gets daily exchange rates from official CNB API.
    /// </summary>
    /// <param name="date">Target date, if date is in future, the latest available exchange rates are returned</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CnbClientExchangeRateResponse> GetDailyExchangeRatesAsync(DateTimeOffset date, CancellationToken cancellationToken);
}
