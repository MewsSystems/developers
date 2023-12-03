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

        var exchangeRates = await _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(buyOrder.SourceCurrency, buyOrder.TargetCurrency, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);

        if (!exchangeRates.Any()) return null;

        var latestExchange = exchangeRates.First();
        var convertedSum = new PositiveRealNumber(buyOrder.SumToExchange * latestExchange.CurrencyRate);

        return new BuyResult(buyOrder.SourceCurrency, buyOrder.TargetCurrency, convertedSum);
    }
}
