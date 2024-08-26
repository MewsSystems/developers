using System;

namespace ExchangeRateProvider.Models
{
    public class CnbRatesModel
    {
        public RateModel[]? Rates { get; set; }

        public class RateModel
        {
            public int Amount { get; set; }
            public required string Country { get; set; }
            public required string Currency { get; set; }
            public required string CurrencyCode { get; set; }
            public int Order { get; set; }
            public decimal Rate { get; set; }
            public DateTime ValidFor { get; set; }
        }
    }
}