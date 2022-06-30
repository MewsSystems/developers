using ExchangeRate.Provider.Base.Interfaces;

namespace ExchangeRate.Provider.Base.Service;

public abstract class ProviderCreator
{
    public abstract IExchangeRateProvider CreateProvider();
}