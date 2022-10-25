using ExchangeRateUpdater.Cnb.Dtos;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ExchangeRateUpdater.Cnb
{
    public class CnbClient : ICnbClient
    {
        private const string TargetCurrency = "CZK";

        private readonly string _url;
        private readonly ILogger<CnbClient> _logger;

        public CnbClient(ILogger<CnbClient> logger, Options options)
        {
            _url = options.Url ?? throw new ArgumentNullException(nameof(Options.Url));
            _logger = logger;
        }

        public async Task<DailyExchangeRates> GetLatestExchangeRatesAsync()
        {
            var content = await _url.GetStringAsync();
            return ParseRates(content);
        }

        /// <summary>
        /// Parses exchange rate according to the specification at https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
        /// </summary>
        private DailyExchangeRates ParseRates(string content)
        {
            var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var dateLine = lines.FirstOrDefault() ?? throw new Exception("No exchange rate information was found.");

            var dateString = dateLine.Split("#").First().Trim();
            var date = DateOnly.Parse(dateString, CultureInfo.InvariantCulture);

            var rates = new List<ExchangeRate>();

            foreach (var line in lines.Skip(2))
            {
                try
                {
                    rates.Add(ParseRate(line));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Could not parse exchange rate info: {line}", line);
                    continue;
                }
            }

            return new DailyExchangeRates(date, rates);
        }

        private ExchangeRate ParseRate(string s)
        {
            var parts = s.Split("|");

            if (parts.Count() < 5)
            {
                throw new Exception("Exchange rate format error");
            }

            return new ExchangeRate(
                Country: parts[0],
                SourceCurrencyName: parts[1],
                Amount: int.Parse(parts[2]),
                SourceCurrencyCode: parts[3],
                TargetCurrencyCode: TargetCurrency,
                Rate: decimal.Parse(parts[4], CultureInfo.InvariantCulture)
                );
        }

        public class Options
        {   
            public string? Url { get; set; }
        }
    }
}
