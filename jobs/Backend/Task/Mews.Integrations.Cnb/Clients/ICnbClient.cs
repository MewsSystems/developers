using System;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Models;

namespace Mews.Integrations.Cnb.Clients;

public interface ICnbClient
{
    Task<CnbClientExchangeRateResponse> GetDailyExchangeRatesAsync(DateTimeOffset date, CancellationToken cancellationToken);
}
