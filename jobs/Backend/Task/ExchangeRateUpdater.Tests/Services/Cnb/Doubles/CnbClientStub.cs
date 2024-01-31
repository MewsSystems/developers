using System;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Clients;
using Mews.Integrations.Cnb.Models;

namespace ExchangeRateUpdater.Tests.Services.Cnb.Doubles;

public class CnbClientStub(CnbClientExchangeRateResponse response) : ICnbClient
{
    public Task<CnbClientExchangeRateResponse> GetDailyExchangeRatesAsync(DateTimeOffset date, CancellationToken cancellationToken)
    {
        return Task.FromResult(response);
    }
}
