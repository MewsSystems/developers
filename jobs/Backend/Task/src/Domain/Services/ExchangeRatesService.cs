using Domain.Entities;
using Domain.Ports;
using Domain.ValueTypes;
using Serilog.Core;

namespace Domain.Services;

public class ExchangeRatesService
{
    private IExchangeRatesRepository _exchangeRatesRepository;
    private readonly Logger _logger;

    public ExchangeRatesService(IExchangeRatesRepository exchangeRatesRepository, Logger logger)
    {
        _exchangeRatesRepository = exchangeRatesRepository ?? throw new ArgumentNullException(nameof(exchangeRatesRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<ExchangeRate>> GetDailyExchangeRates(IEnumerable<ExchangeRateRequest> exchangeRateRequests, DateTime dateToRequest, CancellationToken cancellationToken)
    {
        if (exchangeRateRequests == null) throw new ArgumentNullException(nameof(exchangeRateRequests));
        
        var dailyExchangeRates = await _exchangeRatesRepository.GetDailyExchangeRatesAsync(dateToRequest, cancellationToken);

        if (dailyExchangeRates == null || !dailyExchangeRates.Any())
        {
            return new List<ExchangeRate>();
        }

        // Converting List to dictionary to get better performance on the lookup by exchange rate
        var dailyExchangeRatesDictionary = ConvertExchangeRatesToDictionary(dailyExchangeRates);

        var dailyExchangeRatesByCurrency = new List<ExchangeRate>();

        foreach (var exchangeRate in exchangeRateRequests)
        {
            if (!exchangeRate.SourceCurrency.IsValidCurrencyCode() || !exchangeRate.TargetCurrency.IsValidCurrencyCode())
            {
                _logger.Warning("ExchangeRate {ExchangeRate} is not valid.", exchangeRate.ToString());
            }
            
            else if (dailyExchangeRatesDictionary.TryGetValue(exchangeRate.ToString(), out var value))
            {
                dailyExchangeRatesByCurrency.Add(value);
            }
        }

        return dailyExchangeRatesByCurrency;
    }

    private Dictionary<string, ExchangeRate> ConvertExchangeRatesToDictionary(IEnumerable<ExchangeRate> exchangeRates)
    {
        var exchangeRatesDictionary = new Dictionary<string, ExchangeRate>();

        foreach (var exchangeRate in exchangeRates)
        {
            exchangeRatesDictionary.Add(exchangeRate.ToString(), exchangeRate);
        }

        return exchangeRatesDictionary;
    }
}