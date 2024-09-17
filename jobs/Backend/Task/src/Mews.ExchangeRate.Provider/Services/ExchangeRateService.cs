using Mews.ExchangeRate.Domain.Models;
using Mews.ExchangeRate.Provider.Services.Abstractions;
using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Updater.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ILogger _logger;
        private readonly IExchangeRateQueryRepository _exchangeRateQueryRepository;
        private readonly IExchangeRateUpdateService _exchangeRateUpdateService;

        public ExchangeRateService(
            ILogger<ExchangeRateService> logger,
            IExchangeRateQueryRepository exchangeRateQueryRepository,
            IExchangeRateUpdateService exchangeRateUpdateService
            )
        {
            _logger = logger;
            _exchangeRateQueryRepository = exchangeRateQueryRepository ?? throw new ArgumentNullException(nameof(exchangeRateQueryRepository));
            _exchangeRateUpdateService = exchangeRateUpdateService ?? throw new ArgumentNullException(nameof(exchangeRateUpdateService));
        }

        /// <summary>
        /// Gets the exchange rate for the given source Currency asynchronously.
        /// </summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <returns></returns>
        public async Task<Domain.Models.ExchangeRate> GetExchangeRateAsync(Currency sourceCurrency)
        {
            await ValidateRepositoryReadinessAsync();

            var exchangeRate = await _exchangeRateQueryRepository.GetExchangeRateAsync(sourceCurrency);

            if (exchangeRate is null)
            {
                _logger.LogWarning("No exchange rate found for the given currency {currency}.", sourceCurrency);
                return Domain.Models.ExchangeRate.Empty;
            }

            return exchangeRate;
        }

        /// <summary>
        /// Gets the exchange rates asynchronously.
        /// </summary>
        /// <param name="sourceCurrencies"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Domain.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> sourceCurrencies)
        {
            await ValidateRepositoryReadinessAsync();

            var exchangeRates = await _exchangeRateQueryRepository.GetExchangeRatesAsync(sourceCurrencies);

            if (exchangeRates is null)
            {
                _logger.LogWarning("No exchange rates found for the given currencies {currencies}.", sourceCurrencies);
                return Enumerable.Empty<Domain.Models.ExchangeRate>();
            }

            exchangeRates = exchangeRates.Where(exchangeRate => exchangeRate is not null).ToArray();

            if (exchangeRates.Count() != sourceCurrencies.Count())
            {
                var difference = sourceCurrencies
                    .Except(exchangeRates.Select(exchangeRate => exchangeRate.SourceCurrency))
                    .ToArray();
                
                _logger.LogWarning("Not all exchange rates found for the given currencies. No Exchange Rates available for: {currencies}.", string.Join<Currency>("; ", difference));
            }

            return exchangeRates;
        }

        private async Task ValidateRepositoryReadinessAsync()
        {
            if (!_exchangeRateQueryRepository.IsInitializedAndReady)
            {
                await _exchangeRateUpdateService.RefreshAllExchangeRatesAsync();
            }
        }
    }
}