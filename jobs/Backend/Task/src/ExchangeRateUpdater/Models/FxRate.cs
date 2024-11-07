using System;

namespace ExchangeRateUpdater.Models
{
    public class FxRate
    {
        public DateTime ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
    }
}

