using ExchangeRate.Domain.Providers;

namespace ExchangeRate.Api.Clients;

public interface IExchangeRateClient
{
    public string Name { get; }
    public Uri BaseAddress { get; }

    public Task<IResult> GetExchangeRatesAsync(
        IExchangeRateProviderRequest request,
        CancellationToken cancellationToken);
}