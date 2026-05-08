using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public interface IFxRateService
    {
        Task<IEnumerable<FxRate>> GetFxRatesAsync(DateTime date, string language, CancellationToken cancellationToken);
    }
}
