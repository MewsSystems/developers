using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Const
{

    public class MemoryCacheConstants
    {
        public static string ExchangeRateKey(DateTime? date)
        {
            return $"DATE_{date.GetValueOrDefault().ToString("yyyy-MM-dd")}_KEY";
        }
    }
}
