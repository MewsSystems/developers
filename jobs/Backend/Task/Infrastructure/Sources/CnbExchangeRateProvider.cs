using ExchangeRateUpdater.Domain;
using System.Globalization;

namespace ExchangeRateUpdater.Infrastructure.Sources
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const int HttpTimeoutSeconds = 10;
        private const string TargetCurrency = "CZK";
        private readonly List<string> _endpoints = new();
        private readonly ILogger<CnbExchangeRateProvider> _logger;
    
		public CnbExchangeRateProvider(IConfiguration configuration, ILogger<CnbExchangeRateProvider> logger)
        {
			_endpoints.Add(configuration["ExchangeRateApi:exchangeRateEN"]);
			_endpoints.Add(configuration["ExchangeRateApi:exchangeRateSC"]);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null) throw new ArgumentNullException(nameof(currencies));

            var requestedCurrencies = ExtractRequestedCurrencies(currencies);

            var rawData = FetchRatesFromEndpoints();
            if (string.IsNullOrWhiteSpace(rawData))
                return Enumerable.Empty<ExchangeRate>();

            return ParseRates(rawData, requestedCurrencies);
        }

        private HashSet<string> ExtractRequestedCurrencies(IEnumerable<Currency> currencies)
        {
            return new HashSet<string>(
                currencies.Select(c => c?.Code?.ToUpperInvariant())
                          .Where(s => !string.IsNullOrEmpty(s))
            );
        }

        private string FetchRatesFromEndpoints()
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds) };
            foreach (var url in _endpoints)
            {
                try
                {
                    var response = http.GetAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed requesting CNB rate endpoint {Url}", url);
                }
            }
            _logger.LogError("All CNB rate endpoints failed.");
            return null;
        }

        private IEnumerable<ExchangeRate> ParseRates(string rawData, HashSet<string> requestedCurrencies)
        {
            var results = new List<ExchangeRate>();
            var lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var rate = ParseLine(line, requestedCurrencies);
                if (rate != null)
                    results.Add(rate);
            }
            return results;
        }
        private ExchangeRate ParseLine(string line, HashSet<string> requestedCurrencies)
        {
            try
            {
                if (!line.Contains("|"))
                    return null;

                var parts = line.Split('|');
                if (parts.Length < 5)
                    return null;

                var code = parts[3].Trim().ToUpperInvariant();
                if (!requestedCurrencies.Contains(code))
                    return null;
                var amountText = parts[2].Trim();
                var rateText = parts[4].Trim().Replace(',', '.');

                if (!decimal.TryParse(amountText, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                    amount = 1m;

                if (!decimal.TryParse(rateText, NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
                    return null;

                if (amount != 0m && amount != 1m)
                    rate = rate / amount;
                
                return new ExchangeRate(new Currency(code), new Currency(TargetCurrency), rate);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CNB line parsing failed for line: {Line}", line);
                return null;
            }
        }
    }
}
