using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders;

public interface IExchangeRateProvider
{
    Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(IReadOnlyCollection<string> currencyCodes);
}