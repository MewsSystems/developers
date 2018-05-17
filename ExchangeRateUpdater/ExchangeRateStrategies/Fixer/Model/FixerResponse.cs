using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Model
{
    public class FixerResponse
    {
        public bool Success { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
        public FixerError Error { get; set; }

        public class FixerError
        {
            public int Code { get; set; }
            public string Type { get; set; }
            public string Info { get; set; }
        }
    }
}
