using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO
{
    public class ExchangeRateGetRequest
    {
        public IEnumerable<string> CurrencyCodes { get; set; }
        public DateTime? ExchangeDate { get; set; }
    }
}
