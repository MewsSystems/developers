using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateSource
    {
        Task<ExchangeRate[]> Load();
    }
}
