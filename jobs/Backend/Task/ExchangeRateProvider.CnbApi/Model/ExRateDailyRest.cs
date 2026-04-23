using System;

namespace ExchangeRateUpdated.CnbApi.Model
{
    public partial class ExRateDailyRest
    {
        public long? Amount { get; set; }

        public string? Country { get; set; }

        public string? Currency { get; set; }

        public string? CurrencyCode { get; set; }

        public int? Order { get; set; }

        public double? Rate { get; set; }

        public DateOnly? ValidFor { get; set; }
    }
}
