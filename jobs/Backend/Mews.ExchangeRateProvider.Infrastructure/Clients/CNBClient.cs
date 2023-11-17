using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Mews.ExchangeRateProvider.Infrastructure.Abstractions;
using Mews.ExchangeRateProvider.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mews.ExchangeRateProvider.Infrastructure.Clients
{
    public class CNBClient : ICNBClient
    {
        private readonly CNBClientOptions _cnbClientOptions;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CNBClient> _logger;
        public CNBClient(IOptions<CNBClientOptions> cnbClientOptions, IHttpClientFactory httpClientFactory, ILogger<CNBClient> logger)
        {
            _cnbClientOptions = cnbClientOptions.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        /// <summary>
        /// fetch rates from CNB API and deserialize them
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<IEnumerable<ResponseExchangeRate>> GetDailyRatesCNBAsync(string date, string lang)
        {
            string requestUri = $"{_cnbClientOptions.CnbDailyRatesUrl}?date={date}&lang={lang}";
            var httpClient = _httpClientFactory.CreateClient("CNBClient");
            var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Request to CNB API failed. Status code: {response.StatusCode}";
                _logger.LogError(errorMessage);
                throw new ApplicationException(errorMessage);
            }
            var content = await response.Content.ReadAsStringAsync();
            // json deserialization should be handled in try-catch block
            try
            {
                var result = JsonConvert.DeserializeObject<ResponseExchangeRates>(content)?.Rates!;
                return result;
            }
            catch (JsonReaderException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
            return Enumerable.Empty<ResponseExchangeRate>();
        }
    }
}
