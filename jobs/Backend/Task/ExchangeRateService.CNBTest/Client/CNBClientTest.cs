using ExchangeRateService.CNB.Client;

namespace ExchangeRateServiceTest.Client;

public class CNBClientTest
{
    private readonly ExchangeRate _testExchangeRate1 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now);
    private readonly ExchangeRate _testExchangeRate2 = new (new Currency("EUR"), new Currency("USD"), 0, DateTime.Now.AddDays(-1));
    private CNBClient _client;
    [SetUp]
    public void Setup()
    {
        var logger = NullLogger<CNBClient>.Instance;
        _client = new CNBClient(logger);
    }
}