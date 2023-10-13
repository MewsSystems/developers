using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Connectors
{
    // CNB Api Documentation: https://api.cnb.cz/cnbapi/swagger-ui.html
    public class CzechNationalBankConnector : IBankConnector
    {
        private readonly HttpClient httpClient;

        public CzechNationalBankConnector(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(BankConstants.CzechNationalBank.HttpClientIdentifier);
        }

        public BankIdentifier BankIdentifier => BankIdentifier.CzechNationalBank;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = await httpClient.GetAsync("exrates/daily");
            response.EnsureSuccessStatusCode();

            if (response.Content is null)
                return Enumerable.Empty<ExchangeRate>();

            var exchangeRatesResponse = await response.Content.ReadFromJsonAsync<ExchangeRatesResponse>();

            if (exchangeRatesResponse?.Rates?.Any() != true)
                return Enumerable.Empty<ExchangeRate>();

            var bankRates = exchangeRatesResponse.Rates
                .Where(rate => currencies.Any(currency => currency.Code == rate.CurrencyCode))
                .Select(rate =>
                {
                    var exchangeRateValue = Math.Round(rate.Rate / rate.Amount, 2);
                    return new ExchangeRate(
                        new Currency(rate.CurrencyCode),
                        new Currency(BankConstants.CzechNationalBank.DefaultCurrency),
                        exchangeRateValue
                    );
                })
                .ToList();

            return bankRates;
        }
    }
}
