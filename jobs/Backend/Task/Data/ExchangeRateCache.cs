using System.Collections.Generic;

namespace ExchangeRateUpdater.Data
{
    public sealed class ExchangeRateCache
    {
        public Dictionary<string, List<ExchangeRate>> SourceExchangeRates = new();
        public Dictionary<string, List<ExchangeRate>> TargetExchangeRates = new();
        public int Count { get; set; } = 0;

        public void Add(ExchangeRate exchangeRate)
        {
            AddToDictionary(SourceExchangeRates, exchangeRate.SourceCurrency.Code, exchangeRate);
            AddToDictionary(TargetExchangeRates, exchangeRate.TargetCurrency.Code, exchangeRate);
            Count++;
        }

        private static void AddToDictionary(IDictionary<string, List<ExchangeRate>> dictionary, string code, ExchangeRate exchangeRate)
        {
            if (dictionary.TryGetValue(code, out List<ExchangeRate> list))
            {
                list.Add(exchangeRate);
            }
            else
            {
                dictionary.Add(code, new List<ExchangeRate>() { exchangeRate });
            }
        }
    }
}
