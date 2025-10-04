using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    internal interface IHandler
    {
        ExchangeRate GetExchangeRate(Currency currency);
    }
}
