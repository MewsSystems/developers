using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClientFactory
    {
        IExchangeRateApiClient CreateExchangeRateApiClient(string currencyCode);
    }
}
