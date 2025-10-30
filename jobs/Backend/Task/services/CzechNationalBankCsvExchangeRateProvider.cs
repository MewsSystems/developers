using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.model;

namespace ExchangeRateUpdater.services;

/// <summary>
/// Gets exchange rates from a CSV file provided by the Czech National Bank
/// </summary>
public class CzechNationalBankCsvExchangeRateProvider : IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        throw new System.NotImplementedException();
    }
}