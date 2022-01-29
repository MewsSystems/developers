using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Builders;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Parsers;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient
{
    public class CzechNationalBankApiClient : ICzechNationalBankApiClient
    {
        readonly ICzechNationalBankApiConfigurationProvider _configurationProvider;
        readonly System.Net.Http.HttpClient _httpClient;
        
        public CzechNationalBankApiClient(ICzechNationalBankApiConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            // TODO: inject http client
            _httpClient = new System.Net.Http.HttpClient();
        }
        
        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime dateTime)
        {
            var configuration = _configurationProvider.GetConfiguration();
            
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = configuration.HttpProtocol;
            uriBuilder.Host = configuration.DomainUrl;
            uriBuilder.Path = configuration.Endpoints.ExchangeRatePath;
            uriBuilder.Query = $"date={dateTime.Date.ToString("dd.MM.yyyy")}";

            var uri = uriBuilder.Uri;

            var httpRequestMessage = new HttpRequestMessageBuilder()
                .WithMethod(HttpMethod.Get)
                .WithUrl(uri)
                .Build();

            var response = await CallApiAsync(httpRequestMessage);

            return ExchangeRateResponseParser.Parse(response);
        }
        
        async Task<Stream> CallApiAsync(HttpRequestMessage requestMessage)
        {
            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                var responseStream = await response.Content.ReadAsStreamAsync();

                return responseStream;

            }
            catch (Exception e)
            {
                throw new ApiClientException("Call to the Czech National Bank API wasn't successful", e);
            }
        }
    }
}