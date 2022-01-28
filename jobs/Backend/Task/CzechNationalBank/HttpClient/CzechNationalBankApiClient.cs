using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.CzechNationalBank.Configuration;
using ExchangeRateUpdater.CzechNationalBank.HttpClient.Builders;
using ExchangeRateUpdater.CzechNationalBank.HttpClient.Dtos;
using ExchangeRateUpdater.CzechNationalBank.HttpClient.Parsers;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient
{
    public class CzechNationalBankApiClient : ICzechNationalBankApiClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        
        public CzechNationalBankApiClient()
        {
            _httpClient = new System.Net.Http.HttpClient();
        }
        
        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime dateTime)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = ApiClientConfiguration.HttpProtocol;
            uriBuilder.Host = ApiClientConfiguration.DomainUrl;
            uriBuilder.Path = ApiClientConfiguration.ExchangeRatePath;
            uriBuilder.Query = $"date={dateTime.Date.ToString("dd.MM.yyyy")}";

            var uri = uriBuilder.Uri;

            var httpRequestMessage = new HttpRequestMessageBuilder()
                .WithMethod(HttpMethod.Get)
                .WithUrl(uri)
                .Build();

            var response = await CallApiAsync(httpRequestMessage);

            return ExchangeRateResponseParser.Parse(response);
        }
        
        async Task<string> CallApiAsync(HttpRequestMessage requestMessage)
        {
            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                var stringResponse = await response.Content.ReadAsStringAsync();

                return stringResponse;

            }
            catch (Exception e)
            {
                throw new ApiClientException("Call to the Czech National Bank API wasn't successful", e);
            }
        }
    }
}