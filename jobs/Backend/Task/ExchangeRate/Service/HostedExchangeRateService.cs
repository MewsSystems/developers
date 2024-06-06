using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Controller.Model;
using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Factory;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Repository;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ExchangeRate.Service
{
    /// <summary>
    /// Service responsible for fetching and updating exchange rates.
    /// </summary>
    internal class HostedExchangeRateService(
        IExchangeRateProviderFactory exchangeRateProviderFactory,
        IExchangeRateRepository repository,
        ILogger<HostedExchangeRateService> logger,
        IRecurringJobManager recurringJobManager) : IExchangeRateService
    {
        private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory = exchangeRateProviderFactory ?? throw new ArgumentNullException(nameof(exchangeRateProviderFactory));
        private readonly IExchangeRateRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<HostedExchangeRateService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));

        /// <summary>
        /// Fetches daily exchange rates asynchronously.
        /// </summary>
        public async Task<FetchDailyExchangeRateResponseInternal> GetDailyExchangeRatesAsync(
            FetchDailyExchangeRateRequestInternal request,
            CancellationToken cancellationToken)
        {
            var datasetKey = PrepareDatasetKey(request.BaseCurrency, request.Language, request.Date);

            var cachedData = await _repository.GetExchangeRates(datasetKey);
            if (cachedData != null && cachedData.Date == request.Date)
            {
                return PrepareResponseData(request, cachedData.ExchangeRates);
            }

            var exchangeRateProvider = _exchangeRateProviderFactory.GetProvider(request.BaseCurrency);
            var freshData = await exchangeRateProvider.GetDailyExchangeRates(request.Date, request.Language, cancellationToken);

            if (freshData.Any())
            {
                await _repository.SaveExchangeRates(datasetKey, new ExchangeRateDataset(request.Date, freshData, ExchangeRateDataset.Channel.Direct));
                return PrepareResponseData(request, freshData);
            }
            else
            {
                _logger.LogInformation($"No exchange rate data found for specified date - {request}");
                throw new ExchangeRateUpdaterException("No exchange rate data found for specified date");
            }
        }

        /// <summary>
        /// Updates daily exchange rates for the specified currency and date.
        /// </summary>
        public async Task UpdateDailyExchangeRates(Currency currency, DateOnly date, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Updating exchange rates for {currency} on {date:yyyy-MM-dd}");
                var exchangeRateProvider = _exchangeRateProviderFactory.GetProvider(currency);

                var supportedLanguages = exchangeRateProvider.GetSupportedLanguages();
                foreach (var language in supportedLanguages)
                {
                    var key = PrepareDatasetKey(currency, language, date);
                    var cachedData = await _repository.GetExchangeRates(key);

                    // Update only if the data is not already cached or if the cached data is not from the worker channel
                    if (cachedData == null || cachedData.DataChannel != ExchangeRateDataset.Channel.Worker)
                    {
                        var freshData = await exchangeRateProvider.GetDailyExchangeRates(date, language, cancellationToken);

                        if (freshData.Any())
                        {
                            await _repository.SaveExchangeRates(key, new ExchangeRateDataset(date, freshData, ExchangeRateDataset.Channel.Worker));
                        }
                        else
                        {
                            _logger.LogInformation($"No exchange rate data found for specified date - {currency}, {date:yyyy-MM-dd}");
                        }
                    }
                }

                _logger.LogInformation($"Exchange rates for {currency} on {date:yyyy-MM-dd} updated successfully");
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error while updating exchange rates");
                throw new ExchangeRateUpdaterException("Error fetching exchange rates, please try again", e);
            }
        }

        /// <summary>
        /// Sets up the worker that periodically updates exchange rates using Hangfire.
        /// </summary>
        public void SetupExchangeRateUpdaterWorker()
        {
            _recurringJobManager.AddOrUpdate(
                "CzechNationalBankExchangeRateUpdater",
                () => UpdateDailyExchangeRates(new Currency("CZK"), DateOnly.FromDateTime(DateTime.Today), new CancellationToken()),
                "35-59/5 14 * * *", // Every 5 minutes between 2:35 PM and 3:00 PM
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
                });

            _logger.LogInformation($"{nameof(HostedExchangeRateService)} Worker setup complete");
        }

        /// <summary>
        /// Prepares a dataset key for caching and repository operations.
        /// </summary>
        private string PrepareDatasetKey(Currency baseCurrency, Language language, DateOnly date) =>
            $"{baseCurrency.Code}.{language}.{date:yyyy-MM-dd}";

        /// <summary>
        /// Prepares the response data by filtering exchange rates for target currencies.
        /// </summary>
        private FetchDailyExchangeRateResponseInternal PrepareResponseData(
            FetchDailyExchangeRateRequestInternal request,
            IEnumerable<ExchangeRateData> exchangeRates)
        {
            var filteredExchangeRates = exchangeRates
                .Where(rate => request.TargetCurrencies.Contains(new Currency(rate.CurrencyCode)))
                .ToList();

            return new FetchDailyExchangeRateResponseInternal(request.BaseCurrency, request.Date, filteredExchangeRates);
        }
    }
}
