using Adpater.Http.CzechNationalBank.Model;
using Ardalis.GuardClauses;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Adpater.Http.CzechNationalBank
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

        private static Currency DefaultCurrency = Currency.Create("CZK");

        public CzechNationalBankExchangeRateProvider(IHttpClientFactory httpClientFactory, ILogger<CzechNationalBankExchangeRateProvider> logger)
        {
            _httpClientFactory = Guard.Against.Null(httpClientFactory);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<IEnumerable<ExchangeRate>> GetDailyExchangeRates(Currency target, CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Null(target);

                if (!target.Equals(DefaultCurrency))
                    throw new ArgumentException("Czech National Bank API only exchange to CZN");

                using HttpClient client = _httpClientFactory.CreateClient("CzechNationalBankApi");

                var url = $"{client.BaseAddress}/exrates/daily?lang=EN";

                _logger.LogInformation("Retrieving daily exchange rates from Czech National Bank API", url);

                var exchangeRatesResponse = await client.GetFromJsonAsync<GetDailyExchangeRatesResponse>(url, cancellationToken);

                _logger.LogInformation("{number} exchange rates retrieved from Czech National Bank API", exchangeRatesResponse?.Rates.Length);

                var exchangeRates = exchangeRatesResponse?.Rates
                    .Where(exchange => exchange.Amount > 0)
                    .Select(exchange => ExchangeRate.Create(Currency.Create(exchange.CurrencyCode), target, exchange.Rate / exchange.Amount));

                return exchangeRates ?? Enumerable.Empty<ExchangeRate>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when retrieving information from Czech National Bank API");
                throw;
            }
        }
    }
}
