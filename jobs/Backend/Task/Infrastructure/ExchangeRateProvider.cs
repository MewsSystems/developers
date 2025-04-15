using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.DTOs;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Domain.Validators;
using ExchangeRateUpdater.Infrastructure.Mappers;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IExchangeRateValidator _validator;
        private readonly IExchangeRateMapper _mapper;
        private readonly IOptions<ExchangeRateProviderOptions> _options;

        public CzechNationalBankExchangeRateProvider(HttpClient httpClient, IExchangeRateValidator validator, IExchangeRateMapper mapper, IOptions<ExchangeRateProviderOptions> options)
        {
            _httpClient = httpClient;
            _validator = validator;
            _mapper = mapper;
            _options = options;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var requestUrl = $"{_options.Value.BaseUrl}?date={currentDate}";

            var data = await _httpClient.GetStringAsync(requestUrl);
            var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = JsonSerializer.Deserialize<RateDto>(data, jsonSerializerOptions);

            var validationErrors = _validator.Validate(response);
            if (validationErrors.Any())
            {
                throw new Exception("Data validation failed: " + string.Join(", ", validationErrors));
            }

            var exchangeRates = _mapper.Map(response);
            
            return exchangeRates.Where(er =>
                currencies.Any(currency =>
                    string.Equals(currency.Code, er.TargetCurrency.Code, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
