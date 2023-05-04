using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateModel
    {
       public string Amount { get; set; }

       public string Country { get; set; }

       public string Currency { get; set; }

       public string CurrencyCode { get; set; } 

       public string Rate { get; set; }

    }
}
