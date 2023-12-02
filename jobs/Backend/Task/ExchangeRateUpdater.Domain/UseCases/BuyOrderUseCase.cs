using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.UseCases;

public class BuyOrderUseCase
{
    private IExchangeRateProviderRepository _exchangeRateProviderRepository;

    public BuyOrderUseCase(IExchangeRateProviderRepository exchangeRateProviderRepository)
    {
        _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
    }


    public async Task<BuyResult?> ExecuteAsync(BuyOrder buyOrder)
    {
        if (buyOrder == null) throw new ArgumentNullException(nameof(buyOrder));

        var exchangeRate = await _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(buyOrder.SourceCurrency, buyOrder.TargetCurrency);

        if (exchangeRate == null) return null;

        var convertedSum = new PositiveRealNumber(buyOrder.SumToExchange * exchangeRate.CurrencyRate);

        return new BuyResult(buyOrder.SourceCurrency, buyOrder.TargetCurrency, convertedSum);
    }
}
