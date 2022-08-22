using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Providers;

public class CzechBankExchangeRateProvider : ExchangeRateProviderBase
{
    protected override Currency TargetCurrency => new("CZK");

    public CzechBankExchangeRateProvider(IExchangeRateService exchangeRateService) : base(exchangeRateService)
    {
    }
}