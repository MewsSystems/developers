using ExchangeRateUpdater.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeProviders
{
    public interface IExchangeRateProvider
    {
        Task<Result<ExchangeRates>> GetExchangeRates(IEnumerable<Currency> currencies, DateTimeOffset date);
    }
}
