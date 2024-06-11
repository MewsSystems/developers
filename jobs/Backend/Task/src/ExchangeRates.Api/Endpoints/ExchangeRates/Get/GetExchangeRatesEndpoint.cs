using ExchangeRates.Api.Handlers.Queries.GetExchangeRatesQuery;

namespace ExchangeRates.Api.Endpoints.ExchangeRates.Get;

public class GetExchangeRatesEndpoint : Endpoint<EmptyRequest, IEnumerable<ExchangeRate>>
{
    private readonly IMediator _mediator;

    public GetExchangeRatesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/exchange-rates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetExchangeRatesQuery(), ct);

        if (!result.IsSuccess || result.Value is null)
        {
            await SendErrorsAsync((int)(result.ErrorType ?? ErrorType.InternalError), ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}
