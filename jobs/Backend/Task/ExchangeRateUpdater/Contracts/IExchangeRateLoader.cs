using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateLoader
{
    IRateRefreshScheduler RateRefreshSchedule { get; }
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<string> currencies, DateTime date);
}
