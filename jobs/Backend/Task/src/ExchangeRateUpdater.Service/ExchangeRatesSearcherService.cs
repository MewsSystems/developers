using Domain.Entities;
using Domain.Ports;
using Serilog;

namespace ExchangeRatesSearcherService;

public class ExchangeRatesSearcherService : IExchangeRatesSearcher
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public ExchangeRatesSearcherService(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        throw new NotImplementedException();
    }
}