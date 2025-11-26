using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.UnitTests.Services;

[TestFixture]
public class ExternalBankApiClientTests
{
    [SetUp]
    public void SetUp()
    {
        // set up a mock http message handler, so we don't hit the network during the tests
        _mockHttpMessageHandler = new MockHttpMessageHandler();

        _restClient = new RestClient(new RestClientOptions("http://localhost/")
            { ConfigureMessageHandler = _ => _mockHttpMessageHandler });

        _logger = new NullLogger<ExternalBankApiClient>();

        _fakeSettings = Options.Create(new ExternalBankApiSettings());

        _sut = new ExternalBankApiClient(_fakeSettings, _restClient);
    }

    private MockHttpMessageHandler _mockHttpMessageHandler = default!;
    private RestClient _restClient = default!;
    private NullLogger<ExternalBankApiClient> _logger = default!;
    private ExternalBankApiClient _sut = default!;
    private IOptions<ExternalBankApiSettings> _fakeSettings = default!;

    [Test]
    public async Task GetDailyExchangeRatesAsync_ShouldFireTheHttpCall_WhenCalled()
    {
        // arrange
        var rates = new GetDailyExchangeRatesResponse(new[]
        {
            new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "Australia", "dollar", 1, "AUD",
                15.285m),
            new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "EMU", "euro", 1, "EUR",
                24.935m),
            new GetDailyExchangeRatesResponseItem(DateOnly.Parse("2024-05-10"), 90, "USA", "dollar", 1, "USD",
                23.131m)
        });

        _mockHttpMessageHandler
            .Expect(HttpMethod.Get, "/exrates/daily")
            .Respond("application/json", JsonSerializer.Serialize(rates));

        // act
        var result = await _sut.GetDailyExchangeRatesAsync(null, null, CancellationToken.None);

        // assert
        _mockHttpMessageHandler.VerifyNoOutstandingExpectation();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(rates);
    }
}