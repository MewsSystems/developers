using ERU.Application.DTOs;
using ERU.Application.Interfaces;
using ERU.Application.Services.ExchangeRate;
using Microsoft.Extensions.Options;

namespace ERU.Application.Tests;

[TestFixture]
[Category("Unit")]
[Parallelizable(ParallelScope.All)]
public class CnbDataExtractorTest
{
	private Mock<IHttpClient> _client;
	private Mock<IDataStringParser<IEnumerable<CnbExchangeRateResponse>>> _parser;
	private Mock<IOptions<ConnectorSettings>> _connectorSettings;
	private Mock<ICache> _memoryCacheHelper;

	private CnbDataExtractor _cnbDataExtractor;

	[SetUp]
	public void SetUp()
	{
		_client = new Mock<IHttpClient>();
		_client.Setup(x => x.GetStringAsync(It.IsAny<string>(), new CancellationToken()))
			.Returns(Task.FromResult(string.Join("\n",
				"07 Oct 2022 #195",
				"Country|Currency|Amount|Code|Rate",
				"United Kingdom|pound|1|GBP|28.051",
				"USA|dollar|1|USD|25.028")));

		_parser = new Mock<IDataStringParser<IEnumerable<CnbExchangeRateResponse>>>();
		_parser.Setup(x => x.Parse(It.IsAny<string>()))
			.Returns(new List<CnbExchangeRateResponse> {new(1m, "USD", 25.028m)});

		_connectorSettings = new Mock<IOptions<ConnectorSettings>>();
		_connectorSettings.Setup(x => x.Value).Returns(new ConnectorSettings());

		_memoryCacheHelper = new Mock<ICache>();
		_memoryCacheHelper.Setup(x => x.InsertToCache(It.IsAny<string>(), It.IsAny<string>()));
		_memoryCacheHelper.Setup(x => x.GetFromCache<IEnumerable<CnbExchangeRateResponse>>(It.IsAny<string>()))
			.Returns((IEnumerable<CnbExchangeRateResponse>?)null);

		_cnbDataExtractor = new CnbDataExtractor(_client.Object, _parser.Object, new List<string>() { "url1" }, _memoryCacheHelper.Object);
	}

	[Test]
	[TestCase(new string[] { "USD" }, 1)]
	public async Task ExtractCnbData_WhenCalled_ReturnsExchangeRates(string[] codes, int expectedCount)
	{
		var result = await _cnbDataExtractor.ExtractCnbData(codes, new CancellationToken());
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IEnumerable<CnbExchangeRateResponse>>();
		result.Count().Should().Be(expectedCount);
		result.Should().Satisfy(a => codes.Contains(a.Code));
	}

	[Test]
	public async Task ExtractCnbData_ShouldReturnDataFromCache()
	{
		var cachedData = new List<CnbExchangeRateResponse>
		{
			new(1m, "CZK", 1m)
		};
		_memoryCacheHelper.Setup(x => x.GetFromCache<IEnumerable<CnbExchangeRateResponse>>(It.IsAny<string>()))
			.Returns(cachedData);

		var result = await _cnbDataExtractor.ExtractCnbData(new string[] { "CZK" }, new CancellationToken());
		result.Should().NotBeNull();
		result.Should().BeOfType<List<CnbExchangeRateResponse>>();
		result.Should().BeEquivalentTo(cachedData);
	}
}