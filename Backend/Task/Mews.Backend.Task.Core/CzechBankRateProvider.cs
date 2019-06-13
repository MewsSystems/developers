using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mews.Backend.Task.Core
{
    public class CzechBankRateProvider : IExchangeRateProvider
    {
        private readonly string _targetCurrency = "CZK";
        private readonly IExchageRateParser _exchageRateParser;

        public CzechBankRateProvider(IExchageRateParser exchageRateParser)
        {
            _exchageRateParser = exchageRateParser;
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRateDtos = await _exchageRateParser.GetExchangeRatesAsync();

            return exchangeRateDtos
                .Where(x => currencies.Any(y => y.Code == x.Code))
                .Select(x => new ExchangeRate(new Currency(x.Code), new Currency(_targetCurrency), x.Rate / x.Amount))
                .ToList();
        }
    }
}
