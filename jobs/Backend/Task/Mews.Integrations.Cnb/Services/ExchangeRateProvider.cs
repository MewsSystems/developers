using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Clients;
using Mews.Integrations.Cnb.Contracts.Models;
using Mews.Integrations.Cnb.Contracts.Services;

namespace Mews.Integrations.Cnb.Services;

public class ExchangeRateProvider(ICnbClient cnbClient) : IExchangeRateProvider
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        DateTimeOffset date,
        CancellationToken cancellationToken)
    {
        // TODO use clock
        var exchangeRateResponse = await cnbClient.GetDailyExchangeRatesAsync(date, cancellationToken);
        
        return new List<ExchangeRate>();
    }
}
