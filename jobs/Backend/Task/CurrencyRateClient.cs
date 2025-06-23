using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater;

public interface ICurrencyRateClient
{

    Task<List<ExchangeRate>> GetLatestExchangeRates()
    {
        return GetExchangeRates(DateOnly.FromDateTime(DateTime.Now));
    }

    Task<List<ExchangeRate>> GetExchangeRates(DateOnly forDate);

}