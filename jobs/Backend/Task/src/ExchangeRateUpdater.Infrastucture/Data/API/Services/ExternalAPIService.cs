using ExchangeRateUpdater.Infrastucture.Data.API.Abstractions;
using ExchangeRateUpdater.Infrastucture.Data.API.Entities;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Infrastucture.Data.API.Services
{
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly IExchangeRatesDailyAPIService _externalAPIService;

        public ExternalAPIService(IExchangeRatesDailyAPIService externalAPIService)
        {
            _externalAPIService = externalAPIService;
        }

        public async Task<RatesDTO> GetFromExternalApi()
        {
            var response = await _externalAPIService.GetExternalDataAsync();
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            RatesDTO rates = JsonConvert.DeserializeObject<RatesDTO>(responseBody) ?? new RatesDTO();

            return rates;
        }
    }
}
