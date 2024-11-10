using ExchangeRate.Api.Clients;
using ExchangeRate.Domain.Providers;
using ExchangeRate.Domain.Providers.CzechNationalBank;

namespace ExchangeRate.Api.Endpoints;

public static class ExchangeRateEndpoint
{
    public static void MapExchangeRateEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/cnb",
                ([AsParameters] CzechNationalBankProviderRequest request,
                        [FromKeyedServices(ExchangeRateProviderType.Cnb)]
                        IExchangeRateClient exchangeRateProvider,
                        CancellationToken cancellationToken) =>
                    exchangeRateProvider.GetExchangeRatesAsync(request, cancellationToken))
            .ProducesValidationProblem()
            .Produces<CzechNationalBankProviderResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}