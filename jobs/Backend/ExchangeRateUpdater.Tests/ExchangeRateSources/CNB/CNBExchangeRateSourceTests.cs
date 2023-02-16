using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using static ExchangeRateUpdater.Tests.Constants;

namespace ExchangeRateUpdater.ExchangeRateSources.CNB.Tests;

public class CNBExchangeRateSourceTests
{
    private readonly CNBExchangeRateSource _rateSource;

    public CNBExchangeRateSourceTests()
    {
        var options = Options.Create(new CNBSourceOptions()
        {
            Location = Data.SourceLocation.File,
            FileUri = "../../../Data/denni_kurz.txt"
        });
        _rateSource = new CNBExchangeRateSource(options, NullLogger<CNBExchangeRateSource>.Instance);
    }

    [Fact()]
    public async void LoadAsyncTest()
    {
        await _rateSource.LoadAsync();
    }

    [Fact()]
    public async void GetSourceExchangeRatesTest()
    {
        await _rateSource.LoadAsync();
        var sourceRates = _rateSource.GetSourceExchangeRates(CZK.Code);
        Assert.Empty(sourceRates);
    }

    [Fact()]
    public async void GetTargetExchangeRatesTest()
    {
        await _rateSource.LoadAsync();
        var targetRates = _rateSource.GetTargetExchangeRates(CZK.Code);
        Assert.Equal(31, targetRates.Count());
    }
}