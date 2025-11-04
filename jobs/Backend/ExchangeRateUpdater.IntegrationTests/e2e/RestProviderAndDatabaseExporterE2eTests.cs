using ExchangeRateUpdater.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ExchangeRateUpdater.IntegrationTests.e2e;

public class RestProviderAndDatabaseExporterE2ETests : IClassFixture<TestBase>, IDisposable
{
    private readonly WireMockServer _mockServer;
    private readonly TestBase _fixture;

    public RestProviderAndDatabaseExporterE2ETests(TestBase fixture)
    {
        _fixture = fixture;
        _mockServer = WireMockServer.Start();
    }

    [Fact]
    public async Task Program_WithRestProviderAndDatabaseExporter_ShouldSaveExchangeRatesToDatabase()
    {
        // Arrange
        var jsonResponse = @"{
  ""rates"": [
    {
      ""validFor"": ""2019-05-17"",
      ""order"": 94,
      ""country"": ""Australia"",
      ""currency"": ""dollar"",
      ""amount"": 1,
      ""currencyCode"": ""AUD"",
      ""rate"": 15.858
    },
    {
      ""validFor"": ""2019-05-17"",
      ""order"": 94,
      ""country"": ""EMU"",
      ""currency"": ""euro"",
      ""amount"": 1,
      ""currencyCode"": ""EUR"",
      ""rate"": 25.75
    },
    {
      ""validFor"": ""2019-05-17"",
      ""order"": 94,
      ""country"": ""Thailand"",
      ""currency"": ""baht"",
      ""amount"": 100,
      ""currencyCode"": ""THB"",
      ""rate"": 72.534
    },
    {
      ""validFor"": ""2019-05-17"",
      ""order"": 94,
      ""country"": ""Turkey"",
      ""currency"": ""lira"",
      ""amount"": 1,
      ""currencyCode"": ""TRY"",
      ""rate"": 3.806
    },
    {
      ""validFor"": ""2019-05-17"",
      ""order"": 94,
      ""country"": ""United Kingdom"",
      ""currency"": ""pound"",
      ""amount"": 1,
      ""currencyCode"": ""GBP"",
      ""rate"": 29.395
    }
  ]
}";

        _mockServer
            .Given(Request.Create().WithPath("/cnbapi/exrates/daily").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(jsonResponse));

        Environment.SetEnvironmentVariable("PROVIDER_TYPE", "REST");
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", "DATABASE");
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", _mockServer.Url);
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", _fixture.DbConnectionString);
        Environment.SetEnvironmentVariable("LOG_LEVEL", "Information");
        Environment.SetEnvironmentVariable("CURRENCIES", "AUD,EUR,GBP");

        // Act
        await Program.Main();

        // Assert
        await VerifyDatabaseRecords();
    }

    private async Task VerifyDatabaseRecords()
    {
        var audRate = await _fixture.DbContext.ExchangeRates
            .FirstOrDefaultAsync(r => r.SourceCurrency == "AUD" && r.TargetCurrency == "CZK");
        Assert.NotNull(audRate);
        Assert.Equal(15.858m, audRate.Rate, 3);

        var eurRate = await _fixture.DbContext.ExchangeRates
            .FirstOrDefaultAsync(r => r.SourceCurrency == "EUR" && r.TargetCurrency == "CZK");
        Assert.NotNull(eurRate);
        Assert.Equal(25.75m, eurRate.Rate, 2);

        var gbpRate = await _fixture.DbContext.ExchangeRates
            .FirstOrDefaultAsync(r => r.SourceCurrency == "GBP" && r.TargetCurrency == "CZK");
        Assert.NotNull(gbpRate);
        Assert.Equal(29.395m, gbpRate.Rate, 3);
    }

    public void Dispose()
    {
        _mockServer.Stop();
        _mockServer.Dispose();

        Environment.SetEnvironmentVariable("RATE_PROVIDER_TYPE", null);
        Environment.SetEnvironmentVariable("RATE_EXPORTER_TYPE", null);
        Environment.SetEnvironmentVariable("API_BASE_URL", null);
        Environment.SetEnvironmentVariable("API_ENDPOINT", null);
        Environment.SetEnvironmentVariable("CONNECTION_STRING", null);
        Environment.SetEnvironmentVariable("LOG_LEVEL", null);
        Environment.SetEnvironmentVariable("CURRENCIES", null);
    }
}