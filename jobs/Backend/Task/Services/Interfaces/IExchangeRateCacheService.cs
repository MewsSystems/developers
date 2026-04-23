using ExchangeRateUpdater.Services.Models;
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