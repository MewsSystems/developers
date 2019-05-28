using System;
using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRepository : IExchangeRateRepository
    {
        // Design decisions: uri should be provided by for example from configuration. For simplicity it is defined as private field
        private readonly string getExchangeRatesUri = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private readonly IExchangeRateParsingStrategy parser;
        private readonly IHttpClientFactory clientFactory;

        public CNBExchangeRepository(IExchangeRateParsingStrategy parser, IHttpClientFactory clientFactory)
        {
            this.parser = parser;
            this.clientFactory = clientFactory;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            var exchangeRates = new List<ExchangeRate>();

            var client = this.clientFactory.Create();
            using (var stream = client.GetStreamAsync(this.getExchangeRatesUri).Result)
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    if(this.parser.TryParse(reader.ReadLine(), out var exchangeRate))
                    {
                        exchangeRates.Add(exchangeRate);
                    }
                }
            }

            return exchangeRates;
        }
    }
}
