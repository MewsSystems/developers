using ExchangeRateUpdater.Domain.Model;
using ExchangeRateUpdater.Interface.Configuration;
using ExchangeRateUpdater.Interface.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ExchangeRateUpdater.Implementation.Services
{
    public class CnbApiService : ICnbApiService
    {
        private readonly ILogger<ICnbApiService> _logger;
        private readonly IOptions<CnbSettings> _cnbSettings;

        public CnbApiService(ILogger<ICnbApiService> logger, IOptions<CnbSettings> cnbSettings)
        {
            _logger = logger;
            _cnbSettings = cnbSettings;
        }

        public async Task<IEnumerable<ExchangeRateEntity>> GetExchangeRates()
        { 
            var exchangeRates = Enumerable.Empty<ExchangeRateEntity>();

            var client = new RestClient(_cnbSettings.Value.BaseUrl);
            var request = new RestRequest(_cnbSettings.Value.GetExchangeRatesEndpoint, Method.Get);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content is not null)
            {
                var ratesJson = JObject.Parse(response.Content)["rates"]?.ToString();

                exchangeRates = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateEntity>>(ratesJson);

                _logger.LogInformation($"Successfully retrieved {exchangeRates?.Count()} exchange rates from source.");
            }
            else
            {
                _logger.LogError($"Could not retrieve exchange rates from source.");
            }

            return exchangeRates;
        }
    }
}
