using ExchangeRateService.AutoRegistration;
using ExchangeRateService.Contracts;
using ExchangeRateService.Domain;
using ExchangeRateService.Services;

namespace ExchangeRateService.Api;

internal class ExchangeRateApi : IApiRoute
{ 
    public void Register(RouteGroupBuilder group)
    {
        group
            .MapGet("/exchangeRate", GetExchangeRatesAsync)
            .Produces<ExchangeRateResponse>()
            .WithName("GetExchangeRates")
            .WithOpenApi();
    }

    private async ValueTask<ExchangeRateResponse> GetExchangeRatesAsync(
        [AsParameters] ExchangeRateFilterRequest filter,
        IExchangeRateProvider exchangeRateProvider,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        var currencies = mapper.Map<Currency[]>(filter.CurrencyCode);
        var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies, cancellationToken);
        return mapper.Map<ExchangeRateResponse>(rates);
    }
}