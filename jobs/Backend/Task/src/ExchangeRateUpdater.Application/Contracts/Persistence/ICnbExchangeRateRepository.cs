using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Application.Contracts.Persistence
{
    /// <summary>
    /// Czech National Bank exchange rates repository
    /// </summary>
    public interface ICnbExchangeRateRepository
    {
        /// <summary>
        /// Returns the last valid data for exchange rates for the indicated date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? date, CancellationToken cancellationToken);
    }
}
