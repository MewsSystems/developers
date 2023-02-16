using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.Abstractions
{
    public interface IExchangeRateSource
    {
        public IDictionary<(string sourceCode, string targetCode), ExchangeRate> ExchangeRates { get; }

        public Task LoadAsync();
    }
}