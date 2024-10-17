using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClient
    {
        Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync();
    }
}
