using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Common.Http
{
    /// <summary>
    /// Helper implementation for http operations
    /// </summary>
    public class HttpWrapper : IHttpWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpWrapper(HttpMessageHandler handler = null, bool disposeHandler = true)
        {
            // ensure single instance of http client is used
            _httpClient = handler == null ? new HttpClient() : new HttpClient(handler, disposeHandler);

            // support newest security protocols on each request
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            // Default is 2 minutes: https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.dnsrefreshtimeout(v=vs.110).aspx
            ServicePointManager.DnsRefreshTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

            // Increases the concurrent outbound connections
            ServicePointManager.DefaultConnectionLimit = 1024;
        }

        /// <summary>
        /// Wrapper method to issue http get async
        /// </summary>
        /// <param name="requestUri">The URI of the request</param>
        /// <param name="timeout">Optional timeout for the request</param>
        /// <returns>The response content as a string</returns>
        public async Task<string> HttpGet(string requestUri, TimeSpan? timeout = null)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(requestUri, GetCancellationTokenFromTimeout(timeout)).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            return responseContent;
        }

        /// <summary>
        /// Wrapper method to issue http post async
        /// This method is not used, but is in place to show extendability 
        /// I.e. we might have a client that requires a POST to fetch the response data
        /// </summary>
        /// <param name="requestUri">The URI of the request</param>
        /// <param name="postParameters">Any post parameters for the request</param>
        /// <param name="timeout">Optional timeout for the request</param>
        /// <returns></returns>
        public async Task<string> HttpPost(string requestUri, IEnumerable<KeyValuePair<string, string>> postParameters, TimeSpan? timeout = null)
        {
            using var content = new FormUrlEncodedContent(postParameters);

            HttpResponseMessage responseMessage = await _httpClient.PostAsync(requestUri, content, GetCancellationTokenFromTimeout(timeout)).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            return responseContent;
        }

        /// <summary>
        /// Helper method to setup a CancellationToken (from a custom timeout) for the http request
        /// </summary>
        /// <param name="timeout">Optional, custom timeout</param>
        /// <returns>CancellationToken</returns>
        private CancellationToken GetCancellationTokenFromTimeout(TimeSpan? timeout)
        {
            // set default timeout if not supplied
            if (!timeout.HasValue)
            {
                timeout = TimeSpan.FromSeconds(Constants.DEFAULT_REQUEST_TIMEOUT_IN_SECONDS);
            }

            // https://stackoverflow.com/questions/46874693/re-using-httpclient-but-with-a-different-timeout-setting-per-request
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout.Value);

            return cts.Token;
        }
    }

}
