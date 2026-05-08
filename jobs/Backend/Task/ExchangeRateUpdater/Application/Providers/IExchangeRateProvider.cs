using ExchangeRateUpdater.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Providers
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromDay(IEnumerable<Currency> currencies, string date);
        public Task<IEnumerable<ExchangeRateDifference>> CompareExchangeRatesBetweenDates(IEnumerable<Currency> currencies, string dateA, string dateB);
    }
}
