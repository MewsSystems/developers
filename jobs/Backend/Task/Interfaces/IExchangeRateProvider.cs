using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    HashSet<string> GetActualCurrencyCodes();
    
    Task<List<ExchangeRate>> GetExchangeRates(HashSet<string> requiredCurrencyCodes, CancellationToken cancellationToken);
}