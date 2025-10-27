using ExchangeRateUpdater.Services.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateCacheService
    {
        ICollection<ExchangeRate> GetCachedRates(IEnumerable<string> currencyCodes);
        void SetRates(IEnumerable<ExchangeRate> rates);
        void UpdateInvalidCodes(IEnumerable<string> codes);
        HashSet<string> GetInvalidCodes();
    }
}