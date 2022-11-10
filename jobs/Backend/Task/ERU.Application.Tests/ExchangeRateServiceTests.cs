using ERU.Application.DTOs;
using ERU.Application.Interfaces;
using ERU.Application.Services.ExchangeRate;
using ERU.Domain;

namespace ERU.Application.Tests;

[TestFixture]
[Category("Unit")]
public class ExchangeRateServiceTests
{
	private Mock<IDataExtractor> _httpClientMock;
	private Mock<IContractObjectMapper<CnbExchangeRateResponse, ExchangeRate>> _mapperMock;
	private ExchangeRateService _service;

	[SetUp]
	public void Setup()
	{
		_httpClientMock = new Mock<IDataExtractor>();
		_httpClientMock.Setup(x => x.ExtractCnbData(
			It.IsAny<IReadOnlyCollection<string>>(),
			It.IsAny<CancellationToken>())).ReturnsAsync(
			new List<CnbExchangeRateResponse>
			{
				new(1.0m, "EUR", 27.5m),
				new(10.0m, "USD", 235.0m)
			});
		_mapperMock = new Mock<IContractObjectMapper<CnbExchangeRateResponse, ExchangeRate>>();
		_mapperMock.Setup(x => x.Map<CnbExchangeRateResponse, ExchangeRate>(It.IsAny<CnbExchangeRateResponse>()))
			.Returns((CnbExchangeRateResponse x) =>
			{
				if (x.Code == null || x.Rate == null || x.Amount is null or 0m)
					return null;

				return new ExchangeRate(new Currency("CZK"), new Currency(x.Code), x.Rate.Value / x.Amount.Value);
			});

		_service = new ExchangeRateService(_mapperMock.Object, _httpClientMock.Object);
	}

	[Test]
	public async Task ShouldReturnExchangeRates()
	{
		var result = await _service
			.GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") }, CancellationToken.None);
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
	}

	[Test]
	public async Task ShouldReturnExchangeRatesWithCorrectValues()
	{
		var result = await _service
			.GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") }, CancellationToken.None);
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().OnlyContain(x => x.SourceCurrency.Code == "CZK");
		result.Should().ContainSingle(x => x.TargetCurrency.Code == "EUR" && x.Value == 27.5m);
		result.Should().ContainSingle(x => x.TargetCurrency.Code == "USD" && x.Value == 23.5m);
	}

	[Test]
	public async Task ShouldReturnEmptyListWhenNoData()
	{
		_httpClientMock.Setup(x => x.ExtractCnbData(
			It.IsAny<IReadOnlyCollection<string>>(),
			It.IsAny<CancellationToken>())).ReturnsAsync(Enumerable.Empty<CnbExchangeRateResponse>());

		Assert.ThrowsAsync<ArgumentNullException>(async () => await _service
			.GetExchangeRates(new List<Currency>(), CancellationToken.None));
		var result = await _service
			.GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") }, CancellationToken.None);
		result.Should().BeEmpty();
	}
}