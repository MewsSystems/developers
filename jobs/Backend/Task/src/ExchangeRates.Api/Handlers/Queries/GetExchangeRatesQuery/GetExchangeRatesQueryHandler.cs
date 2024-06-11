using ExchangeRates.Api.Infrastructure.Providers;

namespace ExchangeRates.Api.Handlers.Queries.GetExchangeRatesQuery;

public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, Result<IEnumerable<ExchangeRate>>>
{
    private readonly ILogger<GetExchangeRatesQueryHandler> _logger;
    private readonly IExchangeRatesProvider _exchangeRatesProvider;

    public GetExchangeRatesQueryHandler(ILogger<GetExchangeRatesQueryHandler> logger,
        IExchangeRatesProvider exchangeRatesProvider)
    {
        _logger = logger;
        _exchangeRatesProvider = exchangeRatesProvider;
    }

    public async Task<Result<IEnumerable<ExchangeRate>>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _exchangeRatesProvider.GetExchangeRatesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exchange rates from source");
            throw;
        }
    }
}
