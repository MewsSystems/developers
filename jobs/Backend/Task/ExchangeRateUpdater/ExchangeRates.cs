using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRates
    {
        public ExchangeRate[] Rates { get; set; }
        public DateOnly Date { get; set; }
    }
}
