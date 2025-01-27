using ExchangeRateUpdater.RateSources.CzechNationalBank;

namespace ExchangeRateUpdater.Tests.CzechNationalBank;

public static class TestCommon
{
    public static CzechNationalBankSourceOptions SourceOptions = new()
    {
        MainDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt"),
        SecondaryDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt")
    };
}
