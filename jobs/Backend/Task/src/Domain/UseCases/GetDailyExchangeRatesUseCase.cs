using Domain.Entities;
using Domain.Services;
using Serilog.Core;

namespace Domain.UseCases;

public class GetDailyExchangeRatesUseCase
{
    private readonly ExchangeRatesService _exchangeRatesService;
    private readonly Logger _logger;
        
    public GetDailyExchangeRatesUseCase(ExchangeRatesService exchangeRatesService, Logger logger)
    {
        _exchangeRatesService = exchangeRatesService ?? throw new ArgumentNullException(nameof(exchangeRatesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> ExecuteAsync(IEnumerable<ExchangeRateRequest> exchangeRateRequests, DateTime dateToRequest, CancellationToken cancellationToken)
    {
        if (exchangeRateRequests == null) throw new ArgumentNullException(nameof(exchangeRateRequests));
            
        var exchangeRates = await _exchangeRatesService.GetDailyExchangeRates(exchangeRateRequests, dateToRequest, cancellationToken);
            
        if (!exchangeRates.Any())
        {
            return new List<ExchangeRate>();
        }
            
        return exchangeRates;
    }
}