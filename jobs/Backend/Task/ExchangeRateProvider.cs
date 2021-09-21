using ExchangeRateUpdater.Communication;
using ExchangeRateUpdater.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IParser<string, ExchangeRate> parser;
        private readonly IExchangeRateConfiguration configuration;
        private readonly IHttpsClientAdapter httpClient;

        public ExchangeRateProvider(
            IParser<string, ExchangeRate> parser,
            IExchangeRateConfiguration configuration,
            IHttpsClientAdapter httpClient)
        {
            this.parser = parser;
            this.configuration = configuration;
            this.httpClient = httpClient;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null) throw new ArgumentException();
            var content = string.Empty;
            try
            {
                content = await httpClient.GetAsync(configuration.Url);
            }
            catch (Exception)
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            if (parser.TryParse(content, out var exchangeRates))
            {
                var currencyCodes = new HashSet<string>(currencies.Select(currency => currency.Code));
                return exchangeRates
                    .Where(exchangeRate => currencyCodes.Contains(exchangeRate.SourceCurrency.Code))
                    .ToList();
            }
            else
            {
                throw new InvalidFormatException();
            }
        }
    }
}
