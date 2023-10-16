using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping;
using Mews.ExchangeRateUpdater.Services.Infrastructure;
using Microsoft.Extensions.Configuration;
using Polly;
using ExchangeRateModel = Mews.ExchangeRateUpdater.Services.Models.ExchangeRateModel;

namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB
{
    /// <summary>
    /// This is the class implementation, which calls the external Czech National Bank API to fetch the
    /// exchange rates for different currencies and CZK being the target currency
    /// </summary>
    public class CNBExchangeRatesProvider : IExchangeRateProvider
    {
        private readonly IRestClient _restClient;

        private readonly string _bankApiBaseUrl;
        private readonly int _retryCount;
        private readonly int _retryIntervalInSeconds;

        public CNBExchangeRatesProvider(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _bankApiBaseUrl = configuration["BankApiBaseUrl"] ?? string.Empty;
            _retryCount = int.TryParse(configuration["RetryCount"], out _retryCount) ? _retryCount : 3;
            _retryIntervalInSeconds = int.TryParse(configuration["RetryIntervalInSeconds"], out _retryIntervalInSeconds) ? _retryIntervalInSeconds : 5;
            
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetExchangeRates()
        {
            var bankSourceRatesDto = await SendRequestAndHandleResponse();

            var exchangeRateModelCollection = bankSourceRatesDto?.ToExchangeRateModels();

            return exchangeRateModelCollection ?? new List<ExchangeRateModel>();
        }

        public bool CanProvide(string exchangeRateProvider)
        {
            return string.Equals(exchangeRateProvider, "CNB", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<ExchangeRates?> SendRequestAndHandleResponse()
        {
            // When there is any kind of issue in communicating with the external provider, it uses
            // the retry policy to call the provider configured number of times, which is being read
            // from the appSettings.json file and gives a gap of configured time interval before trying
            // again
            var httpResponseMessage =
                                await Policy.Handle<Exception>(LogExceptionToConsole)
                                .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(_retryIntervalInSeconds))
                                .ExecuteAsync(() => _restClient.Get($"{_bankApiBaseUrl}/cnbapi/exrates/daily", new Dictionary<string, object> { { "lang", "EN" } }));

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorResponse = await _restClient.ReadResponse<ErrorResponse>(httpResponseMessage);
                Console.WriteLine($"The error response is returned from the CNB API with the following details :\n\n Description : {errorResponse.Description}\n Error Code : {errorResponse.ErrorCode}\n Endpoint : {errorResponse.EndPoint}\n Hppened At : {errorResponse.HappenedAt}\n");
                Console.WriteLine("Please try again");
                throw new Exception(httpResponseMessage.ReasonPhrase);
            }

            var exchangeRatesRatesDto = await _restClient.ReadResponse<ExchangeRates>(httpResponseMessage);
            return exchangeRatesRatesDto;
        }

        private bool LogExceptionToConsole(Exception exception)
        {
            Console.WriteLine(exception.Message);

            return true;
        }
    }
}