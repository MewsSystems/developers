using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.Clients;
using ExchangeRates.Contracts;
using ExchangeRates.Parsers;

namespace ExchangeRates.Providers
{
    public class CnbExchangeRateProvider: ICnbExchangeRateProvider
	{        
        private readonly ICnbClient client;
        private readonly ICnbParser parser;

        public CnbExchangeRateProvider(            
			ICnbClient client,
			ICnbParser parser) 
        {            
            this.client = client;
            this.parser = parser;
        }

		/// <inheritdoc>
		public async Task<ExchangeRate[]> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateOnly? day = default, CancellationToken token = default)
        {
            // Rates acquisition
            var ratesData = await client.GetExchangeRatesAsync(day, token);
            if (!string.IsNullOrWhiteSpace(ratesData)) 
            {
                // Rate data pasing
                var rates = parser.ParserData(ratesData);

                // Rate data filtering
                return rates.Where(rate => currencies.Contains(rate.TargetCurrency)).ToArray();
            }

            return Array.Empty<ExchangeRate>();
        }
    }
}
