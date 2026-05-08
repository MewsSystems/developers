using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateSettingsResolver
{
    ExchangeRateSettings ResolveSourceSettings(Currency baseCurrency);
}