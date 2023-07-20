using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Configurations;

public class AppConfigurations
{
    private IEnumerable<Currency>? Currencies { get; set; }
    public IEnumerable<Currency> ValidCurrencies => Currencies?.Where(c=>c.Code!= null) ?? Enumerable.Empty<Currency>();

}