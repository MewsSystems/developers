using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;

using Polly;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Data source retrieving exchange rates by web requests from cnb.cz website.
    /// </summary>
    /// <remarks>
    /// Retrieved data are cached accordingly to cnb.cz instructions.
    /// </remarks>
    public sealed class ExchangeRateDataSource : IExchangeRateDataSource
    {
        private class ExchangeRateData
        {
            public ExchangeRateData(int amount, decimal rate)
            {
                Amount = amount;
                Rate = rate;
            }

            public int Amount { get; }

            public decimal Rate { get; }
        }

        private const string DATA_URL = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const int RETRY_COUNT = 5;
        private const int RETRY_EXPANDER = 50;
        private const string CONTENT_DELIMITER = "\n";
        private const string LINE_DELIMITER = "|";
        private const string TIMESTAMP_KEY = "TimeStamp";

        // HttpClient is flawed, thus the static instance
        // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static readonly HttpClient client = new HttpClient();

        private static readonly TimeSpan serverOffset = TimeSpan.FromHours(1);

        private readonly CultureInfo czechCulture = new CultureInfo("cs-CZ");
        private readonly MemoryCache cache = new MemoryCache("ExchangeRates");

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">In case that no succesfull web request has been made of when data from request were corrupted.</exception>
        public bool TryGet(string currencyCode, out int amount, out decimal exchangeRate)
        {
            if (cache.Get(TIMESTAMP_KEY) is not string)
            {
                LoadAndCacheData();
            }

            if (cache.Get(currencyCode) is ExchangeRateData data)
            {
                exchangeRate = data.Rate;
                amount = data.Amount;

                return true;
            }

            exchangeRate = decimal.Zero;
            amount = 0;

            return false;
        }

        /// <summary>
        /// Loads data by web request and insert them to cache.
        /// </summary>
        /// <exception cref="InvalidOperationException">In case that no succesfull web request has been made of when data from request were corrupted.</exception>
        private void LoadAndCacheData()
        {
            var retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                                    .Or<HttpRequestException>()
                                    .WaitAndRetryAsync(RETRY_COUNT, count => TimeSpan.FromMilliseconds(count * RETRY_EXPANDER));

            var responseMessage = retryPolicy.ExecuteAsync(async () => await client.GetAsync(DATA_URL)).GetAwaiter().GetResult();
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Exchange rate data not available.");
            }

            var responseContent = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var splitted = responseContent.Split(CONTENT_DELIMITER, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!splitted.Any())
            {
                throw new InvalidOperationException("Unexpected data format.");
            }

            var timeStamp = splitted[0];
            var cachePolicy = GetCachePolicy(timeStamp);

            cache.Add(TIMESTAMP_KEY, timeStamp, cachePolicy);

            ParseAndCacheData(cachePolicy, splitted.Skip(2));
        }

        /// <summary>
        /// Returns cache policy based on current date and date retrieved from request.
        /// </summary>
        /// <exception cref="InvalidOperationException">When data from request were corrupted.</exception>
        private CacheItemPolicy GetCachePolicy(string timeStamp)
        {
            timeStamp = timeStamp.Substring(0, 10);

            if (!DateTime.TryParse(timeStamp, czechCulture, DateTimeStyles.AssumeLocal, out var date))
            {
                throw new InvalidOperationException("Unexpected data format.");
            }

            var now = ((DateTimeOffset)DateTime.Now).ToOffset(serverOffset);
            if (now.Date == date)
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(now.Year, now.Month, now.Day + 1, 14, 30, 00, serverOffset)
                };
            }

            return new CacheItemPolicy
            {
                AbsoluteExpiration = now.AddSeconds(1)
            };
        }

        /// <summary>
        /// Parses and caches data from request.
        /// </summary>
        /// <exception cref="InvalidOperationException">When data from request were corrupted.</exception>
        private void ParseAndCacheData(CacheItemPolicy cachePolicy, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var items = line.Split(LINE_DELIMITER);
                if (items.Length != 5)
                {
                    throw new InvalidOperationException("Unexpected data format.");
                }

                var currencyCode = items[3];

                if (int.TryParse(items[2], NumberStyles.Integer, czechCulture, out var amount)
                    && decimal.TryParse(items[4], NumberStyles.Number, czechCulture, out var rate))
                {
                    var data = new ExchangeRateData(amount, rate);
                    cache.Add(currencyCode, data, cachePolicy);
                }
            }
        }
    }
}
