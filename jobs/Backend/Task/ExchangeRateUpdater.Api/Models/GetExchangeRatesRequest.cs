using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Api.Models;

public class GetExchangeRatesRequest
{
    public IEnumerable<string> CurrencyCodes { get; set; } = Array.Empty<string>();
    public DateTime? Date { get; set; }

    public IEnumerable<Currency> ToCurrencies() => CurrencyCodes.Select(code => new Currency(code));
} 