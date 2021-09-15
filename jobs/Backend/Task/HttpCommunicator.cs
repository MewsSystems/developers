using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal class HttpCommunicator : ICommunicator
    {
        private HttpClient _client = new HttpClient();
        private readonly string _baseUrl;
        private readonly string _dateFormat;

        public HttpCommunicator(string baseUrl, string format)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("URL must not be empty.", nameof(baseUrl));
            }
            _baseUrl = baseUrl;

            if(format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }
            _dateFormat = format;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public async Task<string> GetExchangeRateData()
        {
            return await GetExchangeRateData(DateTime.Today);
        }

        public async Task<string> GetExchangeRateData(DateTime day)
        {
            var result = string.Empty;

            var dateFormatted = day.ToString(_dateFormat);
            var urlFormatted = string.Format(_baseUrl, dateFormatted);

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, urlFormatted))
                {
                    var response = await _client.SendAsync(request);
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error while attempting to obtain exchange rate data: {ex.Message}");
            }

            return result;
        }
    }
}