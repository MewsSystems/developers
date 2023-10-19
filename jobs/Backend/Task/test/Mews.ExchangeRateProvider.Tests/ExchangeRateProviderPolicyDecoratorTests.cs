using Moq;
using Polly;
using Polly.Registry;

namespace Mews.ExchangeRateProvider.Tests;

[TestFixture]
public sealed class ExchangeRateProviderPolicyDecoratorTests
{
    private Mock<IExchangeRateProvider> _innerMock;
    private Mock<ResiliencePipelineProvider<string>> _policyMock;

    [SetUp]
    public void SetUp()
    {
        _innerMock = new Mock<IExchangeRateProvider>();

        _innerMock
            .Setup(erp => erp.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

        _policyMock = new Mock<ResiliencePipelineProvider<string>>();

        _policyMock
            .Setup(p => p.GetPipeline("ExchangeRateProviderPolicyDecorator"))
            .Returns(ResiliencePipeline.Empty);
    }

    [Test]
    public async Task Forwards_request_to_inner_policy()
    {
        var sut = new ExchangeRateProviderPolicyDecorator(_innerMock.Object, _policyMock.Object);

        var result = await sut.GetExchangeRatesAsync(new List<Currency> { new("AUD") }, CancellationToken.None);

        _policyMock.Verify(m => m.GetPipeline("ExchangeRateProviderPolicyDecorator"), Times.Once);
        _innerMock.Verify(m => m.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>(), CancellationToken.None), Times.Once);
        Assert.That(result, Is.Not.Null);
    }
}