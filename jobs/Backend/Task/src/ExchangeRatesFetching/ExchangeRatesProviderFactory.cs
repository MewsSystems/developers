using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesFetching;

public interface IExchangeRatesProviderFactory
{
    IExchangeRatesProvider? GetProvider(string bankName);
}

public class ExchangeRatesProviderFactory : IExchangeRatesProviderFactory
{
    private readonly IServiceProvider serviceProvider;

    public ExchangeRatesProviderFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IExchangeRatesProvider? GetProvider(string bankName)
    {
        var providers = serviceProvider.GetServices<IExchangeRatesProvider>();

        return providers.FirstOrDefault(p => string.Equals(p.BankName, bankName, StringComparison.OrdinalIgnoreCase));
    }
}
