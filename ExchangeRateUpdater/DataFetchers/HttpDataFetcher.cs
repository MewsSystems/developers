using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace ExchangeRateUpdater.DataFetchers
{
    /// <summary>
    /// Provides functionality to retrieve raw data from a predifined web location.
    /// </summary>
    public class HttpDataFetcher : IRemoteDataFetcher
    {
        private const int MaxRetries = 3;

        private const string WebLocation = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDataFetcher"/> class,
        /// sets the HTTP client timeout to 10 seconds,
        /// and creates the default retry policy.
        /// </summary>
        public HttpDataFetcher()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            _retryPolicy = CreateDefaultRetryPolicy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDataFetcher"/> class
        /// using the provided <see cref="HttpClient"/> and retry policy.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance used to make HTTP requests.</param>
        /// <param name="retryPolicy">The retry policy applied to HTTP requests.</param>
        public HttpDataFetcher(HttpClient httpClient, AsyncRetryPolicy<HttpResponseMessage> retryPolicy)
        {
            _httpClient = httpClient;
            _retryPolicy = retryPolicy;
        }

        /// <summary>
        /// Fetches raw data from a predefined web location.
        /// </summary>
        /// <returns>String containing raw data ready for parsing.</returns>
        public string FetchData()
        {
            using var response = _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(WebLocation)).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Creates the default retry policy used by the fetcher.
        /// </summary>
        /// <returns>An <see cref="AsyncRetryPolicy{HttpResponseMessage}"/> to be used by the fetcher.</returns>
        private static AsyncRetryPolicy<HttpResponseMessage> CreateDefaultRetryPolicy() => Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(MaxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
