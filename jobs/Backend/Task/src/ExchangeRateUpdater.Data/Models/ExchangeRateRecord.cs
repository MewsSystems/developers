using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Models
{
    /// <summary>
    /// CSV record mapping for exchange rate data from CNB.
    /// </summary>
    class ExchangeRateRecord
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
