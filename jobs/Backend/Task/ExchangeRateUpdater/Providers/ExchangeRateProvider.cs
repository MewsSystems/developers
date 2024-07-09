using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger _logger;

        public CnbExchangeRateProvider(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
            _logger = Log.ForContext<CnbExchangeRateProvider>();
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                _logger.Error("The currencies collection is null or empty.");
                throw new ArgumentException("Currencies collection cannot be null or empty.", nameof(currencies));
            }

            var exchangeRates = new List<ExchangeRate>();

            try
            {
                _logger.Information("Fetching exchange rate data.");
                var data = await _exchangeRateService.FetchExchangeRateDataAsync();
                _logger.Information("Successfully fetched exchange rate data.");

                var lines = data.Split('\n').Skip(2); // Skip headers

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split('|');
                    if (parts.Length < 5)
                    {
                        _logger.Fatal("Unexpected line format: {Line}", line);
                        continue;
                    }

                    var currencyCode = parts[3];
                    if (!decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    {
                        _logger.Fatal("Unable to parse rate for line: {Line}", line);
                        continue;
                    }

                    var targetCurrency = currencies.FirstOrDefault(c => c.Code == currencyCode);
                    if (targetCurrency != null)
                    {
                        exchangeRates.Add(new ExchangeRate(new Currency("CZK"), targetCurrency, rate));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while getting exchange rates.");
                throw;
            }

            return exchangeRates;
        }
    }
}
