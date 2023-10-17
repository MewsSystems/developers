using ExchangeRateUpdater.Domain.Ports;
using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Domain.UseCase
{
    public class GetExchangeRatesUseCase
    {
        private readonly IExchangeRateRepository _cnBApiRepository;
        private readonly ILogger<GetExchangeRatesUseCase> _logger;

        public GetExchangeRatesUseCase(
            IExchangeRateRepository cnBApiRepository,
            ILogger<GetExchangeRatesUseCase> logger)
        {
            _cnBApiRepository = cnBApiRepository ?? throw new ArgumentNullException(nameof(cnBApiRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExchangeRate>> Execute(string defaultCurrency, IEnumerable<CurrencyCode> currencies)
        {
            _logger.LogInformation("Execute GetExchangeRatesUseCase");

            var exchangeRates = await _cnBApiRepository.GetExchangeRates(defaultCurrency, currencies);

            _logger.LogInformation("Success GetExchangeRatesUseCase");

            if (!exchangeRates.Any())
                throw new ValidationException("No valid exchange rates found.");

            return exchangeRates;
        }
    }
}
