using Ardalis.GuardClauses;
using ExchangeRateUpdater.Application.Extensions;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Domain.UseCases;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IGetDailyExchangeRateUseCase _getDailyExchangeRateUseCase;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(IGetDailyExchangeRateUseCase getDailyExchangeRateUseCase, ILogger<ExchangeRateService> logger)
        {
            _getDailyExchangeRateUseCase = Guard.Against.Null(getDailyExchangeRateUseCase);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetDailyExchangeRateForCurrencies(CurrencyDto source,
                                                                                    IEnumerable<CurrencyDto> targetCurrencies,
                                                                                    CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Null(source);
                Guard.Against.NullOrEmpty(targetCurrencies);

                _logger.LogInformation("Retrieving exchange rates from {source} to {targets}", source, targetCurrencies);
                var sourceCurrency = source.ToDomain();
                var targets = targetCurrencies.Select(currency => currency.ToDomain());

                var exchangeRates = await _getDailyExchangeRateUseCase.ExecuteAsync(sourceCurrency, targets, cancellationToken);

                return exchangeRates?.Select(rate => rate.ToDto()) ?? Enumerable.Empty<ExchangeRateDto>();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error when retrieving exchange rates");

                throw;
            }



        }
    }
}
