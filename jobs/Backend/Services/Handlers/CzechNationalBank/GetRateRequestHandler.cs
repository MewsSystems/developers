using Core.Services.CzechNationalBank.Interfaces;
using MediatR;
using Services.Handlers.CzechNationalBank.Requests;
using Services.Handlers.CzechNationalBank.Response;

namespace Services.Handlers.CzechNationalBank;

public class GetRateRequestHandler : IRequestHandler<GetRateRequest, GetRateResponse>
{
    private readonly IExchangeRateService _exchangeRateService;

    public GetRateRequestHandler(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<GetRateResponse> Handle(GetRateRequest request, CancellationToken cancellationToken)
    {
        var response = await _exchangeRateService.GetExchangeRates(request.Currencies);

        return new GetRateResponse(response);
    }
}
