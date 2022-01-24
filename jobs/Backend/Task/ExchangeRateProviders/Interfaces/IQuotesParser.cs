using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.Interfaces
{
    public interface IQuotesParser
    {
        IDictionary<Currency, ExchangeRate> ParseQuotes(Currency targetCurrency, string quotes);
    }
}
