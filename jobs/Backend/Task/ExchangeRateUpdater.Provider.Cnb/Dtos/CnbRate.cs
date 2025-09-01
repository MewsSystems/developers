using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider.Cnb.Dtos
{
    public class CnbRate
    {
        public string ValidFor { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }

    }
}
