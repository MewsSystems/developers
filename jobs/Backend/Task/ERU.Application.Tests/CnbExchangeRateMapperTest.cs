using ERU.Application.DTOs;
using ERU.Application.Services.ExchangeRate;

namespace ERU.Application.Tests;

[TestFixture]
[Category("Unit")]
[Parallelizable(ParallelScope.All)]
public class CnbExchangeRateMapperTest
{
	private CnbExchangeRateMapper _mapper;

    [SetUp]
    public void SetUp()
    {
	    _mapper = new CnbExchangeRateMapper("CZK");
    }

    [Test, TestCaseSource(nameof(TestMapInput))]
    public void TestMap(decimal amount, string code, decimal rate, decimal expectedRate)
    {
	    var result = Contract.Check(new CnbExchangeRateResponse(amount,code, rate), _mapper, "CNB ExchangeRates API response contract failure.");
	    result.Should().NotBeNull();
	    result.SourceCurrency.Should().NotBeNull();
	    result.TargetCurrency.Should().NotBeNull();
	    result.SourceCurrency.Code.Should().Be("CZK");
	    result.TargetCurrency.Code.Should().Be(code);
	    result.Value.Should().Be(expectedRate);
    }
    
    private static IEnumerable<TestCaseData> TestMapInput()
    {
	    yield return new TestCaseData(1.0m, "CZK", 1m, 1m);
	    yield return new TestCaseData(1m, "EUR", 0.39m, 0.39m);
	    yield return new TestCaseData(10m, "USD", 4.6m, 0.46m);
    }
}