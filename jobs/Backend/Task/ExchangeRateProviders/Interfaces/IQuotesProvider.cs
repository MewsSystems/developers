using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.Interfaces
{
    public interface IQuotesProvider
    {
        Task<string> GetQuotesAsync();
    }
}
