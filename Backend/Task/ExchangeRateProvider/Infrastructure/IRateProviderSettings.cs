using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Infrastructure
{
    public interface IRateProviderSettings
    {
        string CZKExchangeRateProviderUrl { get; }
    }
}
