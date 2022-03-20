using ExchangeRateUpdater.helpers;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;
        private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory;

        private const int hoursToReload = 14;
        private const int minutesToReload = 30;

        private (DateTime dateLoaded, IEnumerable<ExchangeRate> exchangeRates) _exchangeRatesStored;

        /// <summary>
        /// Construc ExchangeRateProvider 
        /// </summary>
        public ExchangeRateProvider() : this(
            new HttpClient(),
            LoggerFactory.Create(loggingBuilder => loggingBuilder
            //TODO: use settings file to set LogLevel (ideally use serilog)
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole()),
            new DateTimeProvider()
            )
        { }

        /// <summary>
        /// Construct ExhangeRateProvicer
        /// </summary>
        /// <param name="httpClient"> httpClient </param>
        /// <param name="loggerFactory"> loggerFactory that crates ILogger</param>
        /// <param name="dateTimeProvider"> datetime provider</param>
        public ExchangeRateProvider(HttpClient httpClient, ILoggerFactory loggerFactory, IDateTimeProvider dateTimeProvider)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _exchangeRateProviderFactory = new ExchangeRateProviderFactory(httpClient, loggerFactory);
            _logger = loggerFactory.CreateLogger<ExchangeRateProvider>();
        }


        internal ExchangeRateProvider(HttpClient httpClient, ILoggerFactory loggerFactory, IExchangeRateProviderFactory exchangeRateProviderFactory, IDateTimeProvider dateTimeProvider, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _exchangeRateProviderFactory = exchangeRateProviderFactory ?? throw new ArgumentNullException(nameof(exchangeRateProviderFactory));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// <param name="currencies">List of currencies for which retruns rate</param>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies is null) throw new ArgumentNullException(nameof(currencies));

            try
            {
                var timeToResetInUtc = _dateTimeProvider.GetUtcDate().Date.AddHours(hoursToReload).AddMinutes(minutesToReload);

                if (_exchangeRatesStored.exchangeRates is null || _exchangeRatesStored.dateLoaded > timeToResetInUtc)
                {
                    _logger.LogDebug("Refetching rates");
                    LoadExchangeRates();
                }

                return _exchangeRatesStored.exchangeRates.Where(r => currencies.Any(c => c.Code == r.TargetCurrency.Code));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get exhange rates ex: {ex}");
                return null;
            }
        }

        private void LoadExchangeRates()
        {
            
            IReadOnlyDictionary<string, IRateProvider> providers = _exchangeRateProviderFactory.GetRateProviders();

            List<ExchangeRate> exchangeRates = new();
            foreach (IRateProvider provider in providers.Values)
            {
                IEnumerable<ExchangeRate> rates = provider.GetExchangeRates().GetAwaiter().GetResult();
                exchangeRates.AddRange(rates);
            }

            _exchangeRatesStored = (_dateTimeProvider.GetUtcDate(), exchangeRates);
        }
    }
}
