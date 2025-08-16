using System;

namespace ExchangeRateUpdater.Infrastructure.CNB.Models
{
    internal class ExRateDailyResponse
    {
        public ExRateDaily[] Rates { get; set; } = new ExRateDaily[0];
    }

    internal class ExRateDaily
    {
        public int Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValidFor {  get; set; }
    }
}