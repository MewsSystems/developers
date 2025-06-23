using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank
{
    /// <summary>
    /// Represents a client for interacting with the Czech National Bank API.
    /// </summary>
    internal interface ICzechNationalBankClient
    {
        /// <summary>
        /// Gets daily exchange rates from the Czech National Bank API.
        /// </summary>
        /// <param name="request">The request parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing daily exchange rates.</returns>
        Task<CzechNationalBankDailyExchangeRateResponse> GetDailyExchangeRates(FetchCzechNationalBankDailyExchangeRateRequest request, CancellationToken cancellationToken);
    }
}
