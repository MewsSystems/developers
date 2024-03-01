using ExchangeRates.Domain;

namespace CNB.Client
{

    public interface IBankClient
    {

        Task<List<ExchangeRate>> GetRatesDaily(
           DateOnly date,
            CancellationToken cancellationToken = default);
    }
}