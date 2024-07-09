using ExchangeRateUpdater.Lib.Shared;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public interface IExchangeRateHttpClient
    {
        Task<ProviderExchangeRate> GetCurrentExchangeRateAsync(Currency currency);
    }
}