using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;

namespace ExchangeRateUpdater.Domain.UseCases;

public class BuyOrderUseCase
{
    public IExchangeRateProviderRepository ExchangeRateProviderRepository { get; }

    public BuyOrderUseCase(IExchangeRateProviderRepository exchangeRateProviderRepository)
    {
        ExchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
    }


    public async Task<BuyResult?> ExecuteAsync(BuyOrder orderBuy)
    {
        return null;
    }
}
