using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Services
{
    public class CzechNationalBank : IExchangeRateSource
    {
        private readonly ICzechNationalBankConfig configuration;
        private readonly ICzechNationalBankExchangeRateParser parser;
        private readonly IHttpClientLineReader httpClientLineReader;

        public CzechNationalBank(ICzechNationalBankConfig configuration, ICzechNationalBankExchangeRateParser parser, IHttpClientLineReader httpClientLineReader)
        {
            this.configuration = configuration;
            this.parser = parser;
            this.httpClientLineReader = httpClientLineReader;
        }

        public IEnumerable<ExchangeRate> GetAllExchangeRates()
        {
            var linesWithExchangeRates = httpClientLineReader.ReadLines(configuration.ExchangeRateUrl);
            return parser.ConvertToExchangeRates(linesWithExchangeRates);
        }

    }
}
