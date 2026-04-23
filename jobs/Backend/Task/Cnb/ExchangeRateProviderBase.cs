using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ExchangeRateUpdater.Cnb
{
    public abstract class ExchangeRateProviderBase : IExchangeRateProvider
    {
        protected readonly HttpClient HttpClient;
        protected readonly string _apiUrl;
        protected readonly Currency _baseCurrency;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected ExchangeRateProviderBase(
            IExchangeRateProviderConfiguration config,
            HttpClient httpClient = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            _apiUrl = config.Url;
            _baseCurrency = new Currency(config.BaseCurrency);
            HttpClient = httpClient ?? new HttpClient();
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync<T>(IEnumerable<Currency> currencies)
        {
            try
            {
                var response = await FetchRawDataAsync<T>();
                return MapToExchangeRates(response, currencies);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in GetExchangeRatesAsync");
                throw;
            }
        }

        public async Task<ApiResponse<T>> GetApiDataAsync<T>(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    ApiErrorResponse apiError = GetApiErrorResponse(error);
                    Logger.Error($"API error: {response.StatusCode} - {error}");
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Error = apiError
                    };
                }

                T data = await GetData<T>(response);
                return new ApiResponse<T>
                {
                    Success = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception in GetApiDataAsync");
                return new ApiResponse<T>
                {
                    Success = false,
                    Error = new ApiErrorResponse { Description = $"Exception: {ex.Message}" }
                };
            }
        }

        private static ApiErrorResponse GetApiErrorResponse(string error)
        {
            ApiErrorResponse apiError = null;
            try
            {
                apiError = System.Text.Json.JsonSerializer.Deserialize<ApiErrorResponse>(error);
            }
            catch (Exception jsonEx)
            {
                Logger.Error(jsonEx, $"Failed to deserialize API error response: {error}");
                throw;
            }

            return apiError;
        }

        private static async Task<T> GetData<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            T data = default;
            try
            {
                data = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception jsonEx)
            {
                Logger.Error(jsonEx, $"Failed to deserialize API data response: {json}");
                throw;
            }

            return data;
        }

        protected abstract Task<T> FetchRawDataAsync<T>();
        protected abstract IEnumerable<ExchangeRate> MapToExchangeRates<T>(T rawData, IEnumerable<Currency> currencies);
    }
}
