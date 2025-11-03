using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ExchangeRateHttpClient _client;
        private readonly Currency _baseCurrency = new Currency("CZK");

        public ExchangeRateProvider(ExchangeRateHttpClient client)
        {
            _client = client;
        }
        
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var commonRatesResponse = await _client.GetCommonRatesAsync();
            var otherRatesResponse = await _client.GetOtherRatesAsync();

            var result = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var dto = commonRatesResponse.Rates.FirstOrDefault(r =>
                              string.Equals(r.CurrencyCode, currency.Code, StringComparison.OrdinalIgnoreCase)) ??
                          otherRatesResponse.Rates.FirstOrDefault(r =>
                              string.Equals(r.CurrencyCode, currency.Code, StringComparison.OrdinalIgnoreCase));

                if (dto == null) continue;

                result.Add(new ExchangeRate(
                    sourceCurrency: currency,
                    targetCurrency: _baseCurrency,
                    value: dto.Rate / dto.Amount
                ));
            }

            return result;
        }
    }
}