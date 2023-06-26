using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.DTOs;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    public Task<List<CurrencyReadDTO>> GetExchangeRates();
}