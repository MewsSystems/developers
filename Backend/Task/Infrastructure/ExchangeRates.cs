using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ExchangeRatesCache
    {
        public Dictionary<Tuple<string,string>,ExchangeRate> Exchange { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ValidForDate { get; set; }
    }
}
