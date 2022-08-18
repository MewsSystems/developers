using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateApiServiceClient _exchangeRateApiServiceClient;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(IExchangeRateApiServiceClient exchangeRateApiServiceClient, ILogger<ExchangeRateProvider> logger)
        {
            _exchangeRateApiServiceClient = exchangeRateApiServiceClient;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _logger.LogTrace("Getting exchange rates");
            var result = new List<ExchangeRate>();
            foreach (var currency in currencies.Distinct())
            {
                await GetExchangeRatesForCurrency(currency, result);
            }
            _logger.LogTrace($"{result.Count} exchange rates found");
            return result;
        }

        private async Task GetExchangeRatesForCurrency(Currency currency, List<ExchangeRate> result)
        {
            _logger.LogTrace($"Getting exchange rates for currency {currency}");
            try
            {
                var exchangeRates = await _exchangeRateApiServiceClient.GetExchageRates(currency.Value);
                if (exchangeRates.Rates == null)
                {
                    _logger.LogTrace($"No exchange rates for currency {currency}: {nameof(exchangeRates)}.{nameof(GetExchangeRatesResponse.Rates)} is null");
                    return;
                }

                if (!exchangeRates.Rates.Any())
                {
                    _logger.LogTrace($"No exchange rates for currency {currency}: {nameof(exchangeRates)}.{nameof(GetExchangeRatesResponse.Rates)} is empty");
                    return;
                }
                AddExchangeRates(currency, result, exchangeRates);
                
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception occurred while getting exchange rates for currency {currency}", e);
            }
        }

        private static void AddExchangeRates(Currency currency, List<ExchangeRate> result,
            GetExchangeRatesResponse exchangeRates)
        {
            foreach (var currencyTarget in exchangeRates.Rates!.Keys)
            {
                result.Add(
                    new ExchangeRate(currency, Currency.From(currencyTarget), exchangeRates.Rates[currencyTarget]));
            }
        }
    }
}