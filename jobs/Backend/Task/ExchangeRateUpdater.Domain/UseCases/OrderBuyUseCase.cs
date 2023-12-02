using ExchangeRateUpdater.Domain.Ports;

namespace ExchangeRateUpdater.Domain.UseCases;

internal class OrderBuyUseCase
{
    public IExchangeRateProviderRepository ExchangeRateProviderRepository { get; }

    public OrderBuyUseCase(IExchangeRateProviderRepository exchangeRateProviderRepository)
    {
        ExchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
    }


    public async Task ExecuteAsync()
    {
        
    }
}
