using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Services
{
    public class CzechNationalBank : IExchangeRateSource
    {
        private readonly ICzechNationalBankConfig configuration;
        private readonly IHttpClientLineReader httpClientLineReader;

        public CzechNationalBank(ICzechNationalBankConfig configuration, IHttpClientLineReader httpClientLineReader)
        {
            this.configuration = configuration;

            this.httpClientLineReader = httpClientLineReader;
        }

        public IEnumerable<ExchangeRate> GetAllExchangeRates()
        {
            var linesWithExchangeRates = httpClientLineReader.ReadLines(configuration.ExchangeRateUrl);
            return linesWithExchangeRates.Select(x => new CzechNationalBankExchangeRate(x)).Where(x => x.IsValidExchangeRate).Select(x => x.ExchangeRate).ToEnumerable();
        }

    }
}
