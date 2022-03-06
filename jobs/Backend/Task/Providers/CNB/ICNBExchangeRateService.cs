using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Providers.CNB;

public interface ICNBExchangeRateService
{
    Task<List<ExchangeRate>> GetExchangeRatesAsync();
}