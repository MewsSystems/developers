using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers;

public class ExchangeRateProvidersConfig
{
    public CzechNationalBankExchangeRateConfig? CzechNationalBank { get; init; }
}