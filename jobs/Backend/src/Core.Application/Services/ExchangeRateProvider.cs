using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Models;
using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Core.Application.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<Result<IReadOnlyCollection<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return await _exchangeRateRepository.GetExchangeRates()
                .Map(exchangeRates =>
                    exchangeRates.Value.Where(e =>
                        currencies.Any(c => c.Code == e.SourceCurrency.Code))
                    .ToList().ToReadOnlyCollection());
        }
    }
}
