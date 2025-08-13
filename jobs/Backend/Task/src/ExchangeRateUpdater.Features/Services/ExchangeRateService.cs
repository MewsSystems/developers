using ExchangeRateUpdater.Features.Exceptions;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Features.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(
            IExchangeRateProvider exchangeRateProvider,
            ILogger<ExchangeRateService> logger)
        {
            _exchangeRateProvider = exchangeRateProvider ?? throw new ArgumentNullException(nameof(exchangeRateProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetExchangeRates(IEnumerable<CurrencyModel> currencies)
        {
            if (currencies == null || !currencies.Any())
                throw new ExchangeRateUpdaterException("Currency list can not be null or empty");

            _logger.LogInformation("Handling in function '{function}' with currencies with Id '{id}'",
                nameof(ExchangeRateService.GetExchangeRates), currencies.Select(elem => "'" + elem.Code + "'"));

            var exchangeResults = await _exchangeRateProvider.GetExchangeRates(currencies.ToCurrency());
            return exchangeResults.ToExchangeRateModel();
        }

    }
}
