using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICzechNationalBankClient
    {
        Task<string> GetExchangeRates();
    }
}
