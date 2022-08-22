using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateService
{
    /// <summary>
    /// Get foreign exchange market rates
    /// </summary>
    /// <param name="date">Date of the exchange rate, without parameter, only for current exchange rates</param>
    Task<IReadOnlyCollection<Currency>> GetCurrenciesAsync(DateTime? date = null);
}