using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Model
{
    public class CurrencyLayerResponse
    {
        public bool Success { get; set; }
        public string Source { get; set; }
        public Dictionary<string, decimal> Quotes { get; set; }
        public CurrencyLayerError Error { get; set; }

        public class CurrencyLayerError
        {
            public int Code { get; set; }
            public string Type { get; set; }
            public string Info { get; set; }
        }

        public Dictionary<string, decimal> GetNormalizedRates() => Quotes
            .ToDictionary(item => ParseTargetCurrencyCode(item.Key), item => item.Value);

        private static string ParseTargetCurrencyCode(string currencyCode) => currencyCode.Substring(3, 3);
    }
}
