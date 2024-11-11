using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbCurrencyRateProvider : ICurrencyRateProvider
    {
        private readonly bool _shouldIncludeOtherCurrencies;

        private const string RegularCurrencyExchangeRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string OtherCurrencyExchangeRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

        private static readonly HttpClient _httpClient = new();

        private readonly Currency _sourceCurrency = new("CZK");

        public CnbCurrencyRateProvider(bool shouldIncludeOtherCurrencies)
        {
            _shouldIncludeOtherCurrencies = shouldIncludeOtherCurrencies;
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken)
        {
            var regularCurrenciesResponse = await _httpClient.GetStringAsync(RegularCurrencyExchangeRatesUrl, cancellationToken);
            var resultList = CnbTools.ParseExchangeRates(_sourceCurrency, regularCurrenciesResponse).ToList();

            if (_shouldIncludeOtherCurrencies)
            {
                var otherCurrenciesResponse = await _httpClient.GetStringAsync(OtherCurrencyExchangeRatesUrl, cancellationToken);
                resultList.AddRange(CnbTools.ParseExchangeRates(_sourceCurrency, otherCurrenciesResponse));
            }

            return resultList;
        }
    }
}