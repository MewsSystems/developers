using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.RateProviders
{
    /// <summary>
    /// Currency rate provider for Czech National Bank (aka. CNB)
    /// </summary>
    public class CzechNationalBankRateProvider : IRateProvider
    {
        private const string ProviderName = "CzechNationalBank";
        private const string RowDelimiter = "\n";
        private const string ColumnDelimiter = "|";

        private readonly RateProviderConfiguration _rateProviderConfiguration;
        private readonly HttpClient _httpClient;

        public Currency BaseCurrency => new Currency("CZK");
        public ushort BaseAmmount => 1;

        public CzechNationalBankRateProvider(IEnumerable<RateProviderConfiguration> configurations)
        {
            _rateProviderConfiguration = configurations.FirstOrDefault(c => c.Name == ProviderName);

            if(_rateProviderConfiguration == null)
            {
                throw new Exception($"Missing configuration for '{ProviderName}' rate provider");
            }

            _httpClient = new HttpClient();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CurrencyRateDto>> GetRatesAsync()
        {
            return await GetRatesAsync(DateTime.Now);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CurrencyRateDto>> GetRatesAsync(DateTime relevantDate)
        {
            var url = $"{_rateProviderConfiguration.SourceUrl}?date={relevantDate.ToString("dd.MM.YYYY")}";

            var ratesResponse = await _httpClient.GetAsync(url);

            if(!ratesResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Invalid response from rates API, code {ratesResponse.StatusCode}");
            }

            var responseContent = await ratesResponse.Content.ReadAsStringAsync();

            /*
            * Column design
            * Country name|Currency name|Ammount|Currency code|Exchange rate
            * 
            * Keep in mind that exchange rate have commas as decimal separators
            */
            var rates = responseContent
                .Split(RowDelimiter)
                .Skip(2) // first two rows contains metadata, eg. date, column names
                .Where(r => !string.IsNullOrEmpty(r) && !string.IsNullOrWhiteSpace(r)) // there might be empty rows
                .Select(r => r.Split(ColumnDelimiter))
                .Select(r => new CurrencyRateDto()
                {
                    Ammount = uint.Parse(r[2]),
                    Code = r[3],
                    CountryName = r[0],
                    Name = r[1],
                    Rate = decimal.Parse(r[4].Replace(",", ".")) // exchange comma for dot as decimal separator
                })
                .ToArray();

            return rates;
        }
    }
}
