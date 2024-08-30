using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateUpdater.Services.Client.ClientModel
{
    public class ExchangeRateResponseList
    {
        public IEnumerable<ExchangeRateResponse> Rates { get; set; }
    }
    public class ExchangeRateResponse
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public float Rate { get; set; }
    }
}
