using Cnb.Api.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class CnbApiClientFactory : ICnbApiClientFactory
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CnbApiClientFactory> _logger;
        private readonly string _baseUrl;

        public CnbApiClientFactory(
            HttpClient client,
            IConfiguration configuration,
            ILogger<CnbApiClientFactory> logger
            )
        {
            this._baseUrl = configuration["CnbApiUrl"];
            this._configuration = configuration;
            this._httpClient = client;
            this._logger = logger;
        }

        public ICnbApiClient CnbApiClient
        {
            get { return new CnbApiClient(this._baseUrl, this._httpClient); }
        }
    }
}