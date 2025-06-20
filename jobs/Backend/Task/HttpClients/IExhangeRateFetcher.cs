using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.HttpClients
{
    public interface IExhangeRateFetcher
    {
        Task<string> FetchAsync();
    }
}
