using Mews.ExchangeRate.Http.Abstractions;
using Mews.ExchangeRate.Http.Abstractions.Dtos;
using Mews.ExchangeRate.Http.Cnb.Exceptions;
using Mews.ExchangeRate.Http.Cnb.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Http.Cnb
{
    public class ExchangeRateServiceClient : IExchangeRateServiceClient
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string YearMonthDateFormat = "yyyy-MM";
        private readonly ExchangeRateServiceClientOptions _clientOptions;
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly CultureInfo DefaultCulture = new CultureInfo("cz-CZ");

        public ExchangeRateServiceClient(
            ILogger<ExchangeRateServiceClient> logger,
            IHttpClient httpClient,
            ExchangeRateServiceClientOptions clientOptions
            )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clientOptions = clientOptions ?? throw new ArgumentNullException(nameof(clientOptions));
        }

        /// <summary>
        /// Fetch the Central bank exchange rate fixing for the given date asynchronously .
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ExchangeRateDto>> GetCurrencyExchangeRatesAsync(DateTime date)
        {
            _logger.LogInformation("Fetching the Central bank exchange rate fixing for the date {date}.", date);
            var formattedDate = date.ToString(DateFormat);
            var query = $"date={formattedDate}&lang={_clientOptions.Language?.ToUpper()}";
            var urlBuilder = new UriBuilder(_clientOptions.ApiBaseUrl)
            {
                Path = _clientOptions.CurrencyExchangeRatesEndpoint,
                Query = query
            };

            var response = await _httpClient.GetAsync(urlBuilder.ToString());
            await EnsureSuccessStatusCodeAsync(response);
            var exchangeRates = await DeserializeExchangeRatesAsync(response);
            _logger.LogInformation("CNB exchange rate fixing for the date {date} SUCCESSFULLY fetched.", date);

            return exchangeRates ?? Enumerable.Empty<ExchangeRateDto>();
        }

        /// <summary>
        /// Fetch the Forex rates of other currencies for the given date asynchronously.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ExchangeRateDto>> GetForeignCurrencyExchangeRatesAsync(DateTime date)
        {
            _logger.LogInformation($"Fetching the Central bank foreign currency exchange rate for the date {date}.");

            var formattedDate = date.ToString(YearMonthDateFormat);
            var query = $"yearMonth={formattedDate}&lang={_clientOptions.Language?.ToUpper()}";
            var urlBuilder = new UriBuilder(_clientOptions.ApiBaseUrl)
            {
                Path = _clientOptions.ForeignExchangeRatesEndpoint,
                Query = query
            };

            var response = await _httpClient.GetAsync(urlBuilder.ToString());
            await EnsureSuccessStatusCodeAsync(response);
            var exchangeRates = await DeserializeExchangeRatesAsync(response);

            _logger.LogInformation("Central bank foreign currency exchange rate fixing for the date {date} SUCCESSFULLY fetched.", date);
            return exchangeRates ?? Enumerable.Empty<ExchangeRateDto>();
        }

        /// <summary>
        /// Deserializes the exchange rates.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <exception cref="Mews.ExchangeRate.Http.Cnb.Exceptions.ExchangeRateServiceResponseException">Error while deserializing the CNB exchange rate fixing response.</exception>
        private async Task<IEnumerable<ExchangeRateDto>> DeserializeExchangeRatesAsync(HttpResponseMessage response)
        {
            try
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<ExchangeRateResponse>(responseContent);

                var exchangeRates = rates?.ExchangeRates?.Select(rate => new ExchangeRateDto
                {
                    Amount = rate.Amount,
                    Country = rate.Country,
                    CurrencyCode = rate.CurrencyCode,
                    CurrencyName = rate.Currency,
                    Order = rate.Order,
                    Rate = rate.Rate,
                    ValidFor = DateTime.TryParse(rate.ValidFor, DefaultCulture.DateTimeFormat, DateTimeStyles.None, out DateTime validFor) ? validFor : default
                });

                return exchangeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deserializing the CNB exchange rate fixing response.");
                throw new ExchangeRateServiceResponseException(ex, "Error while deserializing the CNB exchange rate fixing response.");
            }
        }

        /// <summary>
        /// Ensures the success status code.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="Mews.ExchangeRate.Http.Cnb.Exceptions.ExchangeRateServiceResponseException">
        /// Error while fetching the Central bank exchange rate fixing at {date}. Error code:
        /// {errorDetailsErrorCode}. Error description: {errorDetailsDescription}.
        /// </exception>
        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var errorResponseContent = await response.Content.ReadAsStringAsync();
            var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorResponseContent);

            var exception = new ExchangeRateServiceResponseException(
                @"
Error while fetching the CNB exchange rate fixing at {0}.
Error code: {1}.
Error description: {2}.
",
                errorDetails.HappenedAt,
                errorDetails.ErrorCode,
                errorDetails.Description
            );

            _logger.LogError(exception, "Error: {errorResponseContent}", errorResponseContent);

            throw exception;
        }
    }
}