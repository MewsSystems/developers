using ExchangeRatesUpdater.Common;

namespace ExchangeRatesFetching;

public interface IExchangeRatesProvider
{
    string BankName { get; }

    Task<IEnumerable<ExchangeRate>> GetRatesForCurrenciesAsync(IEnumerable<string> currencies);
}
