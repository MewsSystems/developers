using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRateUpdater.Domain.Entities;

namespace Mews.ExchangeRateUpdater.Domain.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencyCodes, DateTime? date);
    }
}
