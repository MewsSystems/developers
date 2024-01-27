using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Clients;
using Mews.Integrations.Cnb.Contracts.Models;

namespace Mews.Integrations.Cnb.Services;

public class ExchangeRateProvider(ICnbClient cnbClient)
{
    public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        DateTimeOffset date,
        CancellationToken cancellationToken)
    {
        // TODO use clock
        var exchangeRateResponse = await cnbClient.GetDailyExchangeRatesAsync(date, cancellationToken);
    }
}
