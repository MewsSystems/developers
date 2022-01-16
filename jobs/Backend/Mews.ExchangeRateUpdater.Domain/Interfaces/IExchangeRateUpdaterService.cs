using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRateUpdater.Domain.Entities;

namespace Mews.ExchangeRateUpdater.Domain.Interfaces
{
    public interface IExchangeRateUpdaterService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencyCodes, DateTime? date);
    }
}
