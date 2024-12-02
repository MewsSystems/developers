using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Mews.ExchangeRate.Http.Abstractions;
using Mews.ExchangeRate.Http.Abstractions.Dtos;
using Mews.ExchangeRate.Http.Cnb.Model;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRate.Http.Cnb
{
    public class ExchangeRateServiceHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateServiceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            return response;
        }
    }    
}
