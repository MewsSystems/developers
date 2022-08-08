using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IRateParser
    {
        public string ForHost();

        public IEnumerable<ExchangeRate> ParseSource(string source);
    }
}