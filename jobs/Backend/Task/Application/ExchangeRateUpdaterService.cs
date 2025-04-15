using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Application;

public interface IExchangeRateUpdaterService
{
    Task RunAsync();
}

public class ExchangeRateUpdaterService : IExchangeRateUpdaterService
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IEnumerable<Currency> _currencies;
    private readonly ILogger<IExchangeRateUpdaterService> _logger;

    public ExchangeRateUpdaterService(IExchangeRateProvider exchangeRateProvider, IOptions<CurrencyOptions> currencyOptions, ILogger<IExchangeRateUpdaterService> logger)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _logger = logger;
        _currencies = currencyOptions.Value.Currencies .Select(code => new Currency(code.Trim())).ToList();
    }
    
    public async Task RunAsync()
    {
        try
        {
            var rates = await _exchangeRateProvider.GetExchangeRates(_currencies);
            _logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                _logger.LogInformation(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not retrieve exchange rates: '{ex.Message}'.");
        }
    }
    
}