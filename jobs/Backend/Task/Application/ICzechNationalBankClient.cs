using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application
{
    public interface ICzechNationalBankClient
    {
        Task<CnbExchangeRateResponse> GetExchangeRatesAsync(CancellationToken cancellationToken);
    }
}
