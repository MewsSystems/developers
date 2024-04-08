using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Interfaces;
using ExchangeRateProvider.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    /// <summary>
    /// Provides methods for interacting with the Czech National Bank (CNB) API to retrieve exchange rates.
    /// </summary>
    public class CnbHttpClient : ICnbHttpClient
    {
        private readonly string _baseApiUrl = "https://api.cnb.cz/cnbapi";
        private readonly HttpClient _httpClient;

        public CnbHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseApiUrl);
        }

        /// <summary>
        /// Async method that retrieves exchange rates for CZK from the CNB API for a specified date and language.
        /// </summary>
        /// <param name="dateTime">The date for which exchange rates are requested.</param>
        /// <param name="language">The language in which the exchange rates should be returned. Default is Czech.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The Result contains the exchange rates as a <see cref="CnbRatesModel"/> object, or <c>null</c> if the retrieval fails.</returns>
        /// <exception cref="HttpRequestException">If the http request is not successful.</exception>
        public async Task<CnbRatesModel?> GetCzkExchangeRatesAsync(DateTime dateTime, string language = Language.Czech, CancellationToken cancellationToken = default)
        {
            var url = $"{_baseApiUrl}/exrates/daily?date={dateTime:yyyy-MM-dd}&lang={language}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CnbRatesModel>(cancellationToken);
        }
    }
}