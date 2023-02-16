using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.Abstractions
{
    public interface IExchangeRateSource
    {

        public Task LoadAsync();
        public IEnumerable<ExchangeRate> GetSourceExchangeRates(string currencyCode);
        public IEnumerable<ExchangeRate> GetTargetExchangeRates(string currencyCode);

    }
}