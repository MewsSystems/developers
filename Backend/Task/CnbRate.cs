using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbRate
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Ammount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
