using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Interfaces;
using MediatR;

namespace ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;

public record GetBatchExchangeRatesQuery(
    BatchRateRequest Request) : IRequest<BatchExchangeRateResponse>;

public class GetBatchExchangeRatesQueryHandler : IRequestHandler<GetBatchExchangeRatesQuery, BatchExchangeRateResponse>
{
    private readonly IExchangeRateService _exchangeRateService;

    public GetBatchExchangeRatesQueryHandler(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<BatchExchangeRateResponse> Handle(GetBatchExchangeRatesQuery request,
        CancellationToken cancellationToken)
    {
        return await _exchangeRateService.GetBatchExchangeRatesAsync(request.Request, cancellationToken);
    }
}