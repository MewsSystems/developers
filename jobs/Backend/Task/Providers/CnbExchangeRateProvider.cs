using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.Clients;
using ExchangeRates.Contracts;
using ExchangeRates.Parsers;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Providers
{
    public class CnbExchangeRateProvider: ICnbExchangeRateProvider
	{
        private readonly ILogger<CnbExchangeRateProvider> logger;
        private readonly ICnbClient client;
        private readonly ICnbParser parser;

        public CnbExchangeRateProvider(
            ILogger<CnbExchangeRateProvider> logger,
			ICnbClient client,
			ICnbParser parser) 
        {
            this.logger = logger;
            this.client = client;
            this.parser = parser;
        }

		/// <inheritdoc>
		public async Task<ExchangeRate[]> GetExchangeRates(IEnumerable<Currency> currencies, DateOnly? day = null)
        {
            // Rates acquisition
            var ratesData = await client.GetExchangeRatesAsync(day);
            if (!string.IsNullOrWhiteSpace(ratesData)) 
            {
                // Rate data pasing
                var rates = parser.ParserData(ratesData);

                // Rate data filtering
                return rates.Where(rate => currencies.Contains(rate.SourceCurrency)).ToArray();
            }

            return Array.Empty<ExchangeRate>();
        }
    }
}
