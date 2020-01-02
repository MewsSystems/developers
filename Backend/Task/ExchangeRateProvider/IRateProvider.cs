using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    public interface IRateProvider
    {
        string SourceCurrencyCode { get; }
        IDictionary<string, decimal> GetExchangeRates(DateTime? date = null);
    }
}
