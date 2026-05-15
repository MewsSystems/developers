using ExchangeRates.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.App.Provider;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
}

