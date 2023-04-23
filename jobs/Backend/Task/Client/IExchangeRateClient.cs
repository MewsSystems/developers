using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
    public interface IExchangeRateClient
    {
        Task<string> GetExchangeRateAsync();
    }
}
