using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.UseCases;

public class ExchangeUseCase
{
    private IExchangeRateProviderRepository _exchangeRateProviderRepository;

    public ExchangeUseCase(IExchangeRateProviderRepository exchangeRateProviderRepository)
    {
        _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
    }


    public async Task<ExchangeResult?> ExecuteAsync(ExchangeOrder buyOrder, DateTime requestDate)
    {
        if (buyOrder == null) throw new ArgumentNullException(nameof(buyOrder));

        var exchangeRates = await _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(buyOrder.SourceCurrency, buyOrder.TargetCurrency, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), requestDate.Date);

        if (!exchangeRates.Any()) return null;

        var latestExchange = exchangeRates.First();
        var convertedSum = new PositiveRealNumber(buyOrder.SumToExchange * latestExchange.CurrencyRate);

        return new ExchangeResult(buyOrder.SourceCurrency, buyOrder.TargetCurrency, convertedSum);
    }
}
