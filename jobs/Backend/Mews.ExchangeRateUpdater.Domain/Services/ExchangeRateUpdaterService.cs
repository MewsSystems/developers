using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRateUpdater.Domain.Entities;
using Mews.ExchangeRateUpdater.Domain.Interfaces;

namespace Mews.ExchangeRateUpdater.Domain.Services
{
    public class ExchangeRateUpdaterService : IExchangeRateUpdaterService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateUpdaterService(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<string> currencyCodes, DateTime? date)
        {
            return _exchangeRateProvider.GetExchangeRates(currencyCodes, date);
        }
    }
}
