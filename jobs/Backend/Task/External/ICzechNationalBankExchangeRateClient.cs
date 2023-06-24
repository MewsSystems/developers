using System.Threading.Tasks;

namespace ExchangeRateProvider.External
{
    public interface ICzechNationalBankExchangeRateClient
    {
        Task<string> GetExchangeRateFixingAsync();
        Task<string> GetFixRatesOfOtherCurrenciesAsync();
    }
}