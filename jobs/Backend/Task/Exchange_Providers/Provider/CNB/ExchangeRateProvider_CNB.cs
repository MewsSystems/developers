using ExchangeRateUpdater.Exchange_Providers.Interfaces;
using ExchangeRateUpdater.Exchange_Providers.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Exchange_Providers.Provider.CNB
{
    internal class ExchangeRateProvider_CNB : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IExchangeRateMapper<CNB_Exchange_Rate> _exchangeRateMapper;
        private readonly IConfiguration _configuration;

        public ExchangeRateProvider_CNB(HttpClient httpClient, IExchangeRateMapper<CNB_Exchange_Rate> exchangeRateMapper, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _exchangeRateMapper = exchangeRateMapper ?? throw new ArgumentNullException(nameof(exchangeRateMapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null)
        {
            try
            {
                var url = $"{_configuration.GetSection("CNB:Base").Value}{_configuration.GetSection("CNB:Daily_Exchange").Value}?lang=EN";
                if (date.HasValue)
                {
                    url += $"&date={(DateTime)date:yyyy-MM-dd}";
                }
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    CNB_Response cnb_response = JsonConvert.DeserializeObject<CNB_Response>(content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

                    List<ExchangeRate> exchangeRates = cnb_response.Rates
                                                        .Select(_exchangeRateMapper.Map)
                                                        .Where(x => currencies.Select(c => c.Code).Contains(x.SourceCurrency.Code))
                                                        .ToList();

                    return exchangeRates;
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { _httpClient.Dispose(); }

            return Enumerable.Empty<ExchangeRate>();
        }
    }
}
