using ExchangeRateUpdater.Models.Requests;
using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Business.Services;
public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateRepository _exchangeRatesRepository;
    private readonly ILogger<ExchangeRateService> _logger;
    public ExchangeRateService(IExchangeRateRepository exchangeRatesRepository, ILogger<ExchangeRateService> logger)
    {
        _exchangeRatesRepository = exchangeRatesRepository;
        _logger = logger;
    }
    public async Task<List<ExchangeRateResultDto>> GetExchangeRates(IEnumerable<ExchangeRateRequest> currencies, DateTime date, CancellationToken cancellationToken)
    {
        var exchangeRatesByCurrency = new List<ExchangeRate>();

        //Get all the rates for a specified date
        var exchangeRates = await _exchangeRatesRepository.GetExchangeRatesByDateAsync(date, cancellationToken);

        //Get only the requested ones
        var requestedExchangeRates = exchangeRates.Where(p => currencies
                            .Any(e => e.ToString() == p.ToString()))
                            .ToList();

        _logger.LogInformation($"{requestedExchangeRates.Count}/{currencies.ToList().Count} exchange rate(s) have been obtained.");
        //Map to a new object that simplifies reading the response
        return requestedExchangeRates.Select(rate => MapToResult(rate)).ToList();
    }

    private ExchangeRateResultDto MapToResult(ExchangeRate rate)
    {
        return new ExchangeRateResultDto(
           new Currency(rate.SourceCurrency.Code),
           new Currency(rate.TargetCurrency.Code),
           rate.Value,
           rate.Date);
    }
}
