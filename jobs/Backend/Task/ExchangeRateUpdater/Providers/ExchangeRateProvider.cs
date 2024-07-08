using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateProvider()
        {
        }

        public ExchangeRateProvider(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();
            var data = await _exchangeRateService.FetchExchangeRateDataAsync();

            var lines = data.Split('\n').Skip(2); // Skip headers
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                var currencyCode = parts[3];
                var rate = decimal.Parse(parts[4], CultureInfo.InvariantCulture);

                var targetCurrency = currencies.FirstOrDefault(c => c.Code == currencyCode);
                if (targetCurrency != null)
                {
                    exchangeRates.Add(new ExchangeRate(new Currency("CZK"), targetCurrency, rate));
                }
            }

            return exchangeRates;
        }
    }
}
