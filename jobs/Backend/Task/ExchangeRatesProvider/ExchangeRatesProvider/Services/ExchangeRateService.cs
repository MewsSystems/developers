using ExchangeRateProvider;
using ExchangeRatesProvider.Interfaces;
using ExchangeRatesProvider.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRatesProvider.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        public ExchangeRateService() { }

        public Currency SourceCurrency = new Currency("CZK");
        private static List<Currency> currencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        public async Task<(RatesViewModel result, int statusCode)> GetExchangeRates()
        {
            var response = new RatesViewModel();
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            //var response = await httpClient.GetAsync($"https://api.cnb.cz/cnbapi/exrates/daily?date={date}");
            try
            {
                using HttpClient httpClient = new()
                {
                    BaseAddress = new Uri("https://api.cnb.cz")
                };
                response = await httpClient.GetFromJsonAsync<RatesViewModel>($"cnbapi/exrates/daily?date={date}&lang=EN");

            }
            catch (Exception ex)
            {
                return (new RatesViewModel(), 400);
            }
            if (response == null)
            {
                return (new RatesViewModel(), 400);
            }
            response.sourceCurrency = SourceCurrency;
            return (response, 200);
        }

        public async Task<(RatesViewModel result, int statusCode)> GetSearchResults(string search)
        {
            var allRates = await GetExchangeRates();
            var searchResult = allRates.result.rates.Where(i => i.currencyCode.Contains(search.ToUpper())).ToList<ExchangeRate>();
            try
            {
                return (new RatesViewModel() { sourceCurrency = allRates.result.sourceCurrency, rates = searchResult }, 200);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return (new RatesViewModel() { sourceCurrency = allRates.result.sourceCurrency }, 400);
        }
        public async Task<(RatesViewModel result, int statusCode)> GetSelectedCurrencies()
        {
            var allRates = await GetExchangeRates();
            var resultList = new List<ExchangeRate>();
            foreach (var currency in currencies)
            {
                var rate = allRates.result.rates.FirstOrDefault(r => r.currencyCode == currency.Code);
                if (rate == null)
                {
                    continue;
                }
                resultList.Add(rate);
            }

            try
            {
                return (new RatesViewModel() { sourceCurrency = allRates.result.sourceCurrency, rates = resultList }, 200);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return (new RatesViewModel() { sourceCurrency = allRates.result.sourceCurrency }, 400);
        }
    }
}
