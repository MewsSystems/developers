using Mews.ExchangeRate.Http.Abstractions;
using Mews.ExchangeRate.Http.Abstractions.Exceptions;
using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Updater.Services.Abstractions;
using Mews.ExchangeRate.Updater.Services.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mews.ExchangeRate.Storage.Abstractions.Models.StorageStatus;

namespace Mews.ExchangeRate.Updater.Services
{
    public class ExchangeRateUpdateService : IExchangeRateUpdateService
    {
        private readonly IClock _clock;
        private readonly IExchangeRateCommandRepository _exchangeRateCommandRepository;
        private readonly IExchangeRateServiceClient _exchangeRateServiceClient;
        private readonly ILogger _logger;

        public ExchangeRateUpdateService(
            ILogger<ExchangeRateUpdateService> logger,
            IClock clock,
            IExchangeRateCommandRepository exchangeRateCommandRepository,
            IExchangeRateServiceClient exchangeRateServiceClient
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _exchangeRateCommandRepository = exchangeRateCommandRepository ?? throw new ArgumentNullException(nameof(exchangeRateCommandRepository));
            _exchangeRateServiceClient = exchangeRateServiceClient ?? throw new ArgumentNullException(nameof(exchangeRateServiceClient));
        }

        /// <summary>
        /// Refreshes all exchange rates asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RefreshAllExchangeRatesAsync()
        {
            return await RefreshAllExchangeRatesAsync(_clock.UtcNow);
        }

        /// <summary>
        /// Refreshes all exchange rates asynchronous.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public async Task<bool> RefreshAllExchangeRatesAsync(DateTime date)
        {
            var result = await Task.WhenAll(
                RefreshCurrencyExchangeRatesAsync(date),
                RefreshForeignCurrencyExchangeRatesAsync(date));

            return Array.TrueForAll(result, r => r);
        }

        /// <summary>
        /// Refreshes the currency exchange rates asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RefreshCurrencyExchangeRatesAsync()
        {
            return await RefreshCurrencyExchangeRatesAsync(_clock.UtcNow);
        }

        /// <summary>
        /// Refreshes the currency exchange rates asynchronous.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public async Task<bool> RefreshCurrencyExchangeRatesAsync(DateTime date)
        {
            var exchangeRates = await GetCurrencyExchangeRatesAsync(date);

            if (exchangeRates is null || !exchangeRates.Any())
            {
                return false;
            }

            await _exchangeRateCommandRepository.SetExchangeRatesAsync(exchangeRates, nameof(IExchangeRateServiceClient.GetCurrencyExchangeRatesAsync), UpdateStatus.Success);
            return true;
        }

        /// <summary>
        /// Refreshes the foreign currency exchange rates asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RefreshForeignCurrencyExchangeRatesAsync()
        {
            return await RefreshForeignCurrencyExchangeRatesAsync(_clock.UtcNow);
        }

        /// <summary>
        /// Refreshes the foreign currency exchange rates asynchronous.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public async Task<bool> RefreshForeignCurrencyExchangeRatesAsync(DateTime date)
        {
            var foreignRates = await GetForeignCurrencyExchangeRatesAsync(date);

            if (foreignRates is null || !foreignRates.Any())
            {
                return false;
            }

            await _exchangeRateCommandRepository.SetExchangeRatesAsync(foreignRates, nameof(IExchangeRateServiceClient.GetForeignCurrencyExchangeRatesAsync), UpdateStatus.Success);

            return true;
        }

        private async Task<IEnumerable<Domain.Models.ExchangeRate>> GetCurrencyExchangeRatesAsync(DateTime date)
        {
            try
            {
                var exchangeRateDtos = await _exchangeRateServiceClient.GetCurrencyExchangeRatesAsync(date);
                var exchangeRates = exchangeRateDtos.Select(dto => dto.ToDomainModel()).ToArray();

                return exchangeRates;
            }
            catch (ExchangeRateServiceResponseException ex)
            {
                _logger.LogError(ex, "Error while fetching exchange rates from CNB API.");
                return Enumerable.Empty<Domain.Models.ExchangeRate>();
            }
        }

        private async Task<IEnumerable<Domain.Models.ExchangeRate>> GetForeignCurrencyExchangeRatesAsync(DateTime date)
        {
            try
            {
                var exchangeRateDtos = await _exchangeRateServiceClient.GetForeignCurrencyExchangeRatesAsync(date);

                if (exchangeRateDtos == null || !exchangeRateDtos.Any())
                {
                    // Retry with the previous month (maybe data is not yet available)
                    exchangeRateDtos = await _exchangeRateServiceClient.GetForeignCurrencyExchangeRatesAsync(date.AddMonths(-1));
                }

                if (exchangeRateDtos is null)
                {
                    return Array.Empty<Domain.Models.ExchangeRate>();
                }

                var exchangeRates = exchangeRateDtos.Select(dto => dto.ToDomainModel()).ToArray();

                return exchangeRates;
            }
            catch (ExchangeRateServiceResponseException ex)
            {
                _logger.LogError(ex, "Error while fetching exchange rates from CNB API.");
                return Enumerable.Empty<Domain.Models.ExchangeRate>();
            }
        }
    }
}