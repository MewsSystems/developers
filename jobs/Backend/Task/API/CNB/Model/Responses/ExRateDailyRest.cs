using System;

namespace ExchangeRateUpdater.API.CNB.Model.Responses
{
    public class ExRateDailyRest
    {
        public long Amount { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public int Order { get; set; }
        public decimal Rate { get; set; }
        public DateTime ValidFor { get; set; }
    }
}
