using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client
{
    internal class CzechNationalBankExchangeClient : ICzechNationalBankExchangeClient
    {
        private readonly ConnectorSettings _connectorSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CzechNationalBankExchangeClient(ConnectorSettings connectorSettings, IHttpClientFactory httpClientFactory, ILogger<CzechNationalBankExchangeClient> logger)
        {
            _connectorSettings = connectorSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProviderExchangeRate>> FetchExchangeRates()
        {
            ValidateRequiredConfigurationAndThrowOnError();

            var providerCultureInfo = new CultureInfo(_connectorSettings.CultureInfo);
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"{_connectorSettings.DailyFileUri}");

            ValidateResponseAndThrowOnErrors(response);

            var responseContent = await response.Content.ReadAsStringAsync();

            var exchangeRates = ParseResponseTextIntoExchangeRates(providerCultureInfo, responseContent);

            return exchangeRates;
        }

        private void ValidateRequiredConfigurationAndThrowOnError()
        {
            if (string.IsNullOrEmpty(_connectorSettings.DailyFileUri))
            {
                throw new Exception($"{nameof(ConnectorSettings)}.{nameof(ConnectorSettings.DailyFileUri)} is missing from configuration");
            }

            if (string.IsNullOrEmpty(_connectorSettings.CultureInfo))
            {
                throw new Exception($"{nameof(ConnectorSettings)}.{nameof(ConnectorSettings.CultureInfo)} is missing from configuration");
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
            var lines = ExchangeRateTextParser.SplitTextResponseIntoLines(responseContent);

            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                var exchangeRate = ExchangeRateTextParser.ParseLine(line, providerCultureInfo, _connectorSettings.CurrencyIndex, _connectorSettings.RateIndex);
                if (exchangeRate != null)
                {
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
