using System.Net;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ExchangeRateUpdater.Lib.Shared;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public class ExchangeRatesParallelHttpClient : IExchangeRatesParallelHttpClient
    {

        private readonly ILogger _logger;
        private readonly IExchangeRateHttpClient _client;
        private readonly IFixedWindowRateLimiter _rateLimiter;
        private readonly SemaphoreSlim _semaphore;

        public ExchangeRatesParallelHttpClient(
            IExchangeRateProviderSettings settings,
            IExchangeRateHttpClient client,
            ILogger logger,
            IFixedWindowRateLimiter rateLimiter
            )
        {
            _client = client;
            _logger = logger;
            _semaphore = new SemaphoreSlim(settings.MaxThreads);
            _rateLimiter = rateLimiter;
        }

        /// <summary>
        /// Get The requested Exchange Rates From The Endpoint in Parallel w/ Rate Limiting
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns>
        /// List of Common Exchange Rates
        /// </returns>
        public async Task<IEnumerable<ProviderExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies
            )
        {
            // create a new client for each request
            var tasks = new List<Task<ProviderExchangeRate>>();
            ICollection<Currency> distinctCurrencies = currencies.Distinct().ToList();

            foreach (var currency in distinctCurrencies)
            {
                await _rateLimiter.WaitAsync(); // Ensure we arent exceeding rate limit specs
                await _semaphore.WaitAsync(); // Share a pool of threads for the operation

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        return await _client.GetCurrentExchangeRateAsync(currency);
                    }
                    finally
                    {
                        _semaphore.Release(); // Ensure the semaphore is released
                    }
                }));
            }

            var exchangeRatesAsync = await Task.WhenAll(tasks);  // Await the completion of all tasks
            var exchangeRates = exchangeRatesAsync
            .Where(exchangeRate => exchangeRate != null)
            .ToList(); // make sure we filter out any unfulfilled requests 

            return exchangeRates;
        }

    }

}