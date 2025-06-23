using ExchangeRateUpdater.Domain.DTO;

namespace ExchangeRateUpdater.Infrastructure.Services.Providers
{
    /// <summary>
    /// Czech National Bank exchange rates API client
    /// </summary>
    public interface ICnbExchangeService
    {
        /// <summary>
        /// Returns the last valid data for exchange rates for the indicated date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesByDateAsync(DateTime? date, CancellationToken cancellationToken);
    }
}
