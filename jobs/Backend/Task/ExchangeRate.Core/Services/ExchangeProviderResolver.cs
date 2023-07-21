using ExchangeRate.Core.Constants;
using ExchangeRate.Core.Providers;
using ExchangeRate.Core.Providers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Core.Services;

public interface IExchangeProviderResolver
{
    IExchangeRateProvider GetProvider(string providerCode);
}

public class ExchangeProviderResolver : IExchangeProviderResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ExchangeProviderResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExchangeRateProvider GetProvider(string providerCode)
    {
        switch (providerCode)
        {
            case ExchangeRateSourceCodes.CzechNationalBank:
                return _serviceProvider.GetService<CnbExchangeRateProvider>();
            default:
                throw new ArgumentException($"Could not find provider for source code {providerCode}", nameof(providerCode));
        }
    }
}
