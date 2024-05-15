using ExchangeRateUpdater.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ExchangeRateUpdater.Domain.UseCases
{
    public interface IGetDailyExchangeRateUseCase
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        Task<IEnumerable<ExchangeRate>> ExecuteAsync(Currency source, IEnumerable<Currency> targetCurrencies, CancellationToken cancellationToken);
    }
}
