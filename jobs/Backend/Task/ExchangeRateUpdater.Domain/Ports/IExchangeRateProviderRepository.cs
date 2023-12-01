namespace ExchangeRateUpdater.Domain.Ports;

public interface IExchangeRateProviderRepository
{
    Task GetDefaultUnitRates();
}