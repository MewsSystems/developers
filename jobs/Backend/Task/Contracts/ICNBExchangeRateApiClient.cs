using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Contracts;

public interface ICNBExchangeRateApiClient
{
    Task<IEnumerable<ExchangeRate>> FetchExchangeRatesAsync(DateOnly date, CancellationToken ct = default);
}
