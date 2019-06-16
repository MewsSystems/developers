using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Model
{
    public class ExchangeRateProviderObject
    {
        public int RateId { get; set; }
        public DateTime RateDate { get; set; }
        public List<ExchangeRateProviderItem> RateItems { get; set; }
    }

    public class ExchangeRateProviderItem
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}
