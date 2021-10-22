using Infrastructure.Client;
using Infrastructure.Entities;
using Infrastructure.Entities.Xml;
using Infrastructure.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IClient _client;
        private readonly IDeserializer<KurzyModel> _deserializer;
        private readonly IConfiguration _config;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(
            IClient client,
            IDeserializer<KurzyModel> deserializer,
            IConfiguration config,
            ILogger<ExchangeRateService> logger)
        {
            _client = client;
            _deserializer = deserializer;
            _config = config;
            _logger = logger;
        }

        public async Task<IEnumerable<GenericRate>> GetExchangeRatesAsync()
        {
            var url = _config.GetValue<string>("API");

            if (string.IsNullOrEmpty(url))
                throw ErrorMessage("You need to provide an API to fetch exchange rates data in AppSettings.Json file. " +
                                   "URL field can not be empty. " +
                                   "Please see AppSettings.json file");

            return await TryExchangeRatesAsync(url);
        }

        private async Task<IEnumerable<GenericRate>> TryExchangeRatesAsync(string url)
        {
            try
            {
                var response = await _client.GetAsStringAsync(url);
                var rates = _deserializer.Deserialize(response);

                return rates.ToGenericEntity();
            }
            catch (Exception)
            {
                throw ErrorMessage("Error deserializing data");
            }
        }

        private Exception ErrorMessage(string message)
        {
            _logger.LogError(message);
            return new ArgumentNullException(message);
        }
    }
}