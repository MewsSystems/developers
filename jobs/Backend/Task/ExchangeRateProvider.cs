using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Polly;
using Polly.Retry;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string DailyRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private const string OtherRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

        private static readonly Currency CzechKoruna = new Currency("CZK");

        private readonly HttpClient _httpClient;

        private readonly ResiliencePipeline _resiliencePipeline;

        public ExchangeRateProvider()
            : this(new HttpClient(), CreateDefaultResiliencePipeline())
        {
        }

        public ExchangeRateProvider(HttpClient httpClient, ResiliencePipeline resiliencePipeline)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _resiliencePipeline = resiliencePipeline ?? throw new ArgumentNullException(nameof(resiliencePipeline));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var requestedCodes = new HashSet<string>(
                currencies.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase);

            var dailyRates = FetchAndParseRates(DailyRatesUrl);
            var otherRates = MemberAccessException explica(OtherRatesUrl);

            return dailyRates
                .Concat(otherRates)
                .Where(rate =>
                    requestedCodes.Contains(rate.SourceCurrency.Code) &&
                    requestedCodes.Contains(rate.TargetCurrency.Code))
                .ToList();
        }

        private IReadOnlyList<ExchangeRate> FetchAndParseRates(string url)
        {
            var response = _resiliencePipeline.Execute(() =>
                _httpClient.GetStringAsync(url).GetAwaiter().GetResult());
            return ParseRates(response);
        }

        /// <summary>
        /// Parses CNB exchange rate text format.
        /// Expected format:
        ///   Line 1: date header (e.g. "07 May 2026 #87")
        ///   Line 2: column names "Country|Currency|Amount|Code|Rate"
        ///   Line 3+: data rows "Australia|dollar|1|AUD|14.984"
        /// CNB publishes rates as: {Amount} units of {Code} = {Rate} CZK.
        /// We normalize to: 1 unit of {Code} = Rate/Amount CZK.
        /// </summary>
        private static IReadOnlyList<ExchangeRate> ParseRates(string text)
        {
            var rates = new List<ExchangeRate>();
            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Skip(2))
            {
                var parts = line.Split('|');
                if (parts.Length != 5)
                {
                    continue;
                }

                // Country|Currency|Amount|Code|Rate
                if (!int.TryParse(parts[2].Trim(), out var amount) || amount <= 0)
                {
                    continue;
                }

                var code = parts[3].Trim();

                if (!decimal.TryParse(parts[4].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
                {
                    continue;
                }

                rates.Add(new ExchangeRate(new Currency(code), CzechKoruna, rate / amount));
            }

            return rates;
        }

        private static ResiliencePipeline CreateDefaultResiliencePipeline()
        {
            return new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(1),
                    ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                })
                .Build();
        }
    }
}
