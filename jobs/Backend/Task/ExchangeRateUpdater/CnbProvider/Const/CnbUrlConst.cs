using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider.Const
{
    public static class CnbUrlConst
    {
        public const string DailyUrl = @"https://api.cnb.cz/cnbapi/exrates/daily";
        public const string MonthlyUrl = @"https://api.cnb.cz/cnbapi/fxrates/daily-month";
    }
}
