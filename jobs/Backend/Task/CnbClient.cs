using Polly;
using Polly.Retry;
using System;
using System.IO;
using System.Net.Http;

namespace ExchangeRateUpdater
{

    internal class CnbClient : ICnbClient
    {
        private readonly CnbSettings _settings;
        private readonly HttpClient _client = new();
        private readonly RetryPolicy<string> _retryPolicy;

        public CnbClient(CnbSettings settings)
        {
            _settings = settings;
            _retryPolicy = Policy<string>
                .Handle<HttpRequestException>()
                .Or<IOException>()
                .WaitAndRetry(_settings.RetryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public (bool Success, string RawData) GetRawDailyExchanges()
        {
            string rawData;

            try
            {
                rawData = _retryPolicy.Execute(ReadRawData);
            }
            catch (HttpRequestException)
            {
                return (false, string.Empty);
            }
            catch (IOException)
            {
                return (false, string.Empty);
            }

            return (true, rawData);
        }

        private string ReadRawData()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _settings.SourceUrl);
            using var response = _client.Send(request);

            response.EnsureSuccessStatusCode();

            using var reader = new StreamReader(response.Content.ReadAsStream());
            return reader.ReadToEnd();
        }
    }
}
