using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    internal sealed class CNBExchangeRateFetcher(
        HttpClient httpClient, 
        IExchangeRateParser parser,
        ILogger<CNBExchangeRateFetcher> logger) : IExchangeRateFetcher
    {
        private const string RatesSegment = "daily.txt";

        private readonly HttpClient httpClient = httpClient;
        private readonly IExchangeRateParser parser = parser;

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken cancellationToken = default)
        {
            logger.LogTrace("Getting exchange rates from CNB");

            try
            {
                var response = await httpClient.GetAsync(RatesSegment, cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                return parser.Parse(content).Records;
            }
            catch (BrokenCircuitException)
            {
                // We would want to retrieve the info from other source or use "old" data. 
                // Returning empty collection for simplicy here.
                logger.LogWarning("Circuit open when fetching data. Using alternative path");

                return [];
            }
        }
    }
}
