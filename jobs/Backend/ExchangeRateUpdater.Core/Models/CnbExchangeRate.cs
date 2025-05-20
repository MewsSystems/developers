using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Models
{
    public class CnbExchangeRate
    {
        public string Country { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Rate { get; set; }
    }
}
