using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Configuration.Options;

public class CurrencyOptions
{
    public Currency[] Currencies { get; set; } = [];
}