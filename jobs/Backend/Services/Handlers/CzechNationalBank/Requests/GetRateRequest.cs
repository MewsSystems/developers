using Data;
using MediatR;
using Services.Handlers.CzechNationalBank.Response;

namespace Services.Handlers.CzechNationalBank.Requests;

public class GetRateRequest : IRequest<GetRateResponse>
{
    public GetRateRequest(List<Currency> currencies)
    {
        Currencies = currencies;
    }

    public List<Currency> Currencies { get; }
}
