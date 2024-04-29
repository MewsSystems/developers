using ErrorOr;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Infrastructure
{
    public interface IExchangeRateProviderRepository
    {
        Task<ErrorOr<IEnumerable<ExchangeRate>>> GetCentralBankRates(CancellationToken cancellationToken);
    }
}
