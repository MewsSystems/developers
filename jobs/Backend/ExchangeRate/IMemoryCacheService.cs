using CzechNationalBankClient.Model;

namespace ExchangeRateProvider
{
    public interface IMemoryCacheService
    {
        IEnumerable<CnbExchangeRate>? GetCachedRatesValue(string key);
        void SetCachedRates(string key, IEnumerable<CnbExchangeRate> rates);
    }
}
