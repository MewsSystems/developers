using Domain.Entities;
using Domain.Ports;
using Serilog;

namespace Domain.UseCases;

public class GetExchangeRatesUseCase
{
    private readonly IExchangeRatesSearcher _exchangeRatesSearcherService;
    private readonly ILogger _logger;
    
    private static readonly IEnumerable<Currency> _currencies = new[]
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

    public GetExchangeRatesUseCase(IExchangeRatesSearcher exchangeRatesSearcherService, ILogger logger)
    {
        _exchangeRatesSearcherService = exchangeRatesSearcherService ?? throw new ArgumentNullException(nameof(exchangeRatesSearcherService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var exchangeRates = await _exchangeRatesSearcherService.GetExchangeRates(DateTime.Now);
        
            _logger.Information($"Successfully retrieved {exchangeRates.Count()} exchange rates:");

            var filteredExchangeRates = exchangeRates
                .Where(r => _currencies.Any(c => c.Code == r.SourceCurrency.Code));

            var filteredRecordsCount = exchangeRates.Count() - filteredExchangeRates.Count();
            _logger.Information($"Filtered {filteredRecordsCount} exchange rates from the retrieved list");

            foreach (var rate in filteredExchangeRates)
            {
                _logger.Information(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
        }
    }
}