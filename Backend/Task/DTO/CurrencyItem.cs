using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.DTO
{
    public class CurrencyItem
    {
        public string Code { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }

        public decimal Rate { get; set; }
        public int Amount { get; set; }
        public decimal SingleVal
        {
            get
            {
                return Rate / Amount;
            }
        }
    }
}
