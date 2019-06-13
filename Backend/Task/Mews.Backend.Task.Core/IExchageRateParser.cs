using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mews.Backend.Task.Core
{
    public interface IExchageRateParser
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync();
    }
}
