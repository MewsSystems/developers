using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ExchangeRateResponse
    {
        public IEnumerable<ExchangeRateData> Rates { get; set; }
    }

    public class ExchangeRateData
    {
        public int Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValidFor { get; set; }
    }
}
