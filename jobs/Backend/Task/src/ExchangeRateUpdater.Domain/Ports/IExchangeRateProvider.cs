using ExchangeRateUpdater.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Ports
{
    public interface IExchangeRateProvider
    {
        
        Task<IEnumerable<ExchangeRate>> GetDailyExchangeRates(Currency target, CancellationToken cancellationToken);
    }
}
