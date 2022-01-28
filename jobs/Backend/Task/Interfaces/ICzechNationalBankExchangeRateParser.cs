using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface ICzechNationalBankExchangeRateParser
    {
        public IEnumerable<ExchangeRate> ConvertToExchangeRates(IAsyncEnumerable<string> linesWithExchangeRates);
    }
}
