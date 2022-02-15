using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRates
    {
        public ExchangeRates(IEnumerable<ExchangeRate> rates, DateTimeOffset date)
        {
            Rates = rates;
            Date = date;
        }

        public DateTimeOffset Date { get; set; }

        public IEnumerable<ExchangeRate> Rates { get; set; }
    }
}
