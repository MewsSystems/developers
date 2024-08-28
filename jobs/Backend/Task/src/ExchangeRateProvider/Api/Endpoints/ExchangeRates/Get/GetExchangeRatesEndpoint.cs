using ExchangeRateUpdater.Application.Handlers.QueryHandlers.Abstract;
using ExchangeRateUpdater.Domain.Entities;
using FastEndpoints;
using System.Net;

namespace ExchangeRateUpdater.Api.Endpoints.ExchangeRates.Get;

public class GetExchangeRatesEndpoint(IAsyncQueryHandler<IEnumerable<ExchangeRate>> getSupportedExchangeRatesQueryHandler) : EndpointWithoutRequest<GetExchangeRatesResponse>
{
    private readonly IAsyncQueryHandler<IEnumerable<ExchangeRate>> _getSupportedExchangeRatesQueryHandler = getSupportedExchangeRatesQueryHandler;

    public override void Configure()
    {
        Get("/exchange-rates");
        Throttle(hitLimit: 60, durationSeconds: 60);
        AllowAnonymous();
        Summary(es =>
        {
            es.Summary = "Get all supported exchange rates";
            es.Description = "Returns the different exchange rates based on the currencies predefined in the configuration";
        });
        Description(b => b
            .Produces<GetExchangeRatesResponse>((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.TooManyRequests),
            clearDefaults: true);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var rates = await _getSupportedExchangeRatesQueryHandler.HandleAsync();
        Response = new GetExchangeRatesResponse() { ExchangeRates = rates };
    }
}
