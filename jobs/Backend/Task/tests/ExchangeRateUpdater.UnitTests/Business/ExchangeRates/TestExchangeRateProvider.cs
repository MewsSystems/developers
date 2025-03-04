using ExchangeRateUpdater.Business.ExchangeRates;
using ExchangeRateUpdater.Dto.ExchangeRates;
using ExchangeRateUpdater.Infrastructure.CNB;
using ExchangeRateUpdater.Infrastructure.CNB.Entities;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Business;

public class TestExchangeRateProvider
{
    private class TestState
    {
        public Mock<ICnbProvider> CnbProvider { get; }
        public ExchangeRateProvider Subject { get; }

        public TestState()
        {
            CnbProvider = new Mock<ICnbProvider>();
            Subject = new ExchangeRateProvider(CnbProvider.Object);
        }

        public CnbExchangeRateEntity GetDefaultRate(string code, decimal rate)
        {
            return new CnbExchangeRateEntity()
            {
                Amount = 1,
                Country = "fakeCountry",
                Currency = "fakeCurrency",
                Order = 1,
                CurrencyCode = code,
                ValidFor = DateOnly.FromDateTime(DateTime.UtcNow),
                Rate = rate
            };
        }
    }

    [Fact]
    public async Task WhenUserSendEmptyList_ThenNotQueryRateProvider()
    {
        TestState state = new TestState();
        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        CnbExchangeRateEntity rate2 = state.GetDefaultRate("BBB", 111.22m);
        CnbExchangeRateEntity rate3 = state.GetDefaultRate("CCC", 4444.11m);
        state.CnbProvider.Setup(a => a.GetLatestExchangeInformation())
            .ReturnsAsync(new CnbExchangeResponseEntity() { Rates = [rate1, rate2, rate3] });

        List<ExchangeRate> result =
            await state.Subject.GetExchangeRates([]);

        Assert.Empty(result);

        state.CnbProvider.Verify(a => a.GetLatestExchangeInformation(), Times.Never);
    }


    [Fact]
    public async Task WhenRateDoesNotExistInList_ThenGetsIgnored()
    {
        TestState state = new TestState();
        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        CnbExchangeRateEntity rate2 = state.GetDefaultRate("BBB", 111.22m);
        state.CnbProvider.Setup(a => a.GetLatestExchangeInformation())
            .ReturnsAsync(new CnbExchangeResponseEntity() { Rates = [rate1, rate2] });

        List<ExchangeRate> result = await state.Subject.GetExchangeRates([new Currency("CCC"), new Currency("AAA")]);
    
        Assert.Single(result);
        AssertExchangeRates(rate1, result[0]);

        state.CnbProvider.Verify(a => a.GetLatestExchangeInformation(), Times.Once);
    }

    [Fact]
    public async Task WhenRatesAreInList_ThenReturnRate()
    {
        TestState state = new TestState();
        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        CnbExchangeRateEntity rate2 = state.GetDefaultRate("BBB", 111.22m);
        CnbExchangeRateEntity rate3 = state.GetDefaultRate("CCC", 4444.11m);
        state.CnbProvider.Setup(a => a.GetLatestExchangeInformation())
            .ReturnsAsync(new CnbExchangeResponseEntity() { Rates = [rate1, rate2, rate3] });

        List<ExchangeRate> result =
            await state.Subject.GetExchangeRates([new Currency(rate1.CurrencyCode), new Currency(rate3.CurrencyCode)]);

        Assert.Equal(2, result.Count);
        AssertExchangeRates(rate1, result[0]);
        AssertExchangeRates(rate3, result[1]);

        state.CnbProvider.Verify(a => a.GetLatestExchangeInformation(), Times.Once);
    }


    [Fact]
    public async Task WhenCaseIsNotTheSame_ThenReturnCorrectValues()
    {
        TestState state = new TestState();
        CnbExchangeRateEntity rate1 = state.GetDefaultRate("AAA", 123.41m);
        state.CnbProvider.Setup(a => a.GetLatestExchangeInformation())
            .ReturnsAsync(new CnbExchangeResponseEntity() { Rates = [rate1] });

        List<ExchangeRate> result =
            await state.Subject.GetExchangeRates([new Currency("aAA")]);

        Assert.Single(result);
        AssertExchangeRates(rate1, result[0]);

        state.CnbProvider.Verify(a => a.GetLatestExchangeInformation(), Times.Once);
    }


    private void AssertExchangeRates(CnbExchangeRateEntity expected, ExchangeRate actual)
    {
        Assert.Equal(ExchangeRateConstants.SourceCurrencyExchangeRate, actual.SourceCurrency.Code);
        Assert.Equal(expected.CurrencyCode, actual.TargetCurrency.Code);
        Assert.Equal(expected.Rate, actual.Value);
    }
}