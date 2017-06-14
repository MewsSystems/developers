using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public enum ExchangeRateType
    {
        BuyInCash,
        SellInCash,
        MiddleInCash,
        BuyTransfer,
        SellTransfer,
        MiddleTransfer,
        CentralBank
    }
}
