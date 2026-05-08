using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks; 
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Interfaces;

public interface IExchangeRateRepository
{
    /// <summary>
    /// GET current CNB rates.
    /// </summary>
    Task<IReadOnlyDictionary<string, ExchangeRate>> GetCurrentRatesAsync(CancellationToken cancellationToken = default);
}
