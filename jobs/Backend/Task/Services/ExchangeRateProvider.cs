using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Builders;
using ExchangeRateUpdater.Services.Clients;
using ExchangeRateUpdater.Services.Handlers;
using ExchangeRateUpdater.Services.Parsers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider
    {
        private readonly ICnbApiClient _apiClient;
        private readonly ICnbDataParser _parser;
        private readonly IExchangeRateBuilder _builder;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(
            ICnbApiClient apiClient,
            ICnbDataParser parser,
            IExchangeRateBuilder builder,
            ILogger<ExchangeRateProvider> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies));
            }

            var currencyList = currencies.ToList();
            if (!currencyList.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            _logger.LogInformation("Getting exchange rates for {Count} currencies", currencyList.Count);

            try
            {
                var result = _apiClient.GetExchangeRateDataAsync(CancellationToken.None).GetAwaiter().GetResult();

                if (result.IsFailed)
                {
                    var error = ErrorHandler.ExtractError(result);
                    _logger.LogError("Failed to get exchange rates: {ErrorCode} - {Message}",
                        error.ErrorCode, error.Message);
                    throw error;
                }

                var parsedDataResult = _parser.Parse(result.Value);
                if (parsedDataResult.IsFailed)
                {
                    var error = ErrorHandler.ExtractError(parsedDataResult);
                    _logger.LogError("Failed to parse exchange rates: {ErrorCode} - {Message}",
                        error.ErrorCode, error.Message);
                    throw error;
                }

                var rates = _builder.BuildExchangeRates(currencyList, parsedDataResult.Value);
                _logger.LogInformation("Successfully retrieved {Count} exchange rates", rates.Count());

                return rates;
            }
            catch (CnbException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retrieving exchange rates");
                throw new CnbException(CnbErrorCode.UnexpectedError, "Failed to process exchange rates", ex);
            }
        }
    }
}
