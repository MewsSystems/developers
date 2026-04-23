using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Infrastucture.Data.API.Abstractions;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Infrastucture.Data.API.Services
{
    public class ExchangeRatesDailyAPIService : IExchangeRatesDailyAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ExchangeRatesDailyAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> GetExternalDataAsync()
        {
            var baseUrl = _configuration["ExternalApiSettings:BaseUrlExchangeRatesDaily"];
            var language = _configuration["ExternalApiSettings:Language"] ?? "EN";
            Language lang = (Language)Enum.Parse(typeof(Language), language);
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{baseUrl}?lang={lang}");

            return response;
        }
    }
}
