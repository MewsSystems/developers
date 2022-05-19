using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.RatesReader
{
    public interface IAllCurrentRatesReaderService
    {
        Task<Result<IEnumerable<CurrencyExchangeRate>>> GetAllExchangeRates();
    }
}
