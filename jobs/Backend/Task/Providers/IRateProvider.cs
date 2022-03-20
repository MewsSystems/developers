using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    internal interface IRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates();
        public string BaseCurrencyCode { get; }
    }
}