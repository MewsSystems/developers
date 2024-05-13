using ExchangeRate.Datalayer.Configuration;
using ExchangeRate.Datalayer.Models;
using ExchangeRate.Datalayer.Services;
using ExchangeRates.Providers.CzechNationalBank.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Providers.CzechNationalBank.Service
{
    internal class CzechNationalBankExchangeService : IExchangeRateService
    {
        private readonly IBaseApiSettings _baseApiSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CzechNationalBankExchangeService(IBaseApiSettings baseApiSettings, IHttpClientFactory httpClientFactory, ILogger<CzechNationalBankExchangeService> logger)
        {
            _baseApiSettings = baseApiSettings;
            _httpClientFactory = httpClientFactory;
           
        }

        public async Task<IEnumerable<ProviderExchangeRate>> GetExchangeRates()
        {
            ValidateRequiredConfigurationAndThrowOnError();

            var providerCultureInfo = new CultureInfo(_baseApiSettings.CultureInfo);
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"{_baseApiSettings.DailyFileUri}");

            ValidateResponseAndThrowOnErrors(response);

            var responseContent = await response.Content.ReadAsStringAsync();

            var exchangeRates = ParseResponseTextIntoExchangeRates(providerCultureInfo, responseContent);

            return exchangeRates;
        }

        private void ValidateRequiredConfigurationAndThrowOnError()
        {
            if (string.IsNullOrEmpty(_baseApiSettings.DailyFileUri))
            {
                throw new Exception($"{nameof(BaseApiSettings)}.{nameof(BaseApiSettings.DailyFileUri)} is missing from configuration");
            }

            if (string.IsNullOrEmpty(_baseApiSettings.CultureInfo))
            {
                throw new Exception($"{nameof(BaseApiSettings)}.{nameof(BaseApiSettings.CultureInfo)} is missing from configuration");
            }
        }

        private static void ValidateResponseAndThrowOnErrors(HttpResponseMessage response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Invalid response received from provider server, expected {System.Net.HttpStatusCode.OK}, received {response.StatusCode}");
            }
        }

        private List<ProviderExchangeRate> ParseResponseTextIntoExchangeRates(CultureInfo providerCultureInfo, string responseContent)
        {
            var exchangeRates = new List<ProviderExchangeRate>();
            var lines = ResponseTextParser.SplitTextResponseIntoLines(responseContent);

            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                var exchangeRate = ResponseTextParser.ParseLine(line, providerCultureInfo, _baseApiSettings.CurrencyIndex, _baseApiSettings.RateIndex);
                if (exchangeRate != null)
                {
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
