using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Configurations;

public class AppConfigurations
{
    public IEnumerable<Currency> Currencies { get; set; }
}