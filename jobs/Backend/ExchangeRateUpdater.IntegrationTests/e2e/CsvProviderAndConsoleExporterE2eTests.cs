using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ExchangeRateUpdater.IntegrationTests.e2e;

public class CsvProviderAndConsoleExporterE2ETests : IDisposable
{
    private readonly WireMockServer _mockServer;
    private readonly StringWriter _consoleOutput;
    private readonly TextWriter _originalConsoleOut;

    public CsvProviderAndConsoleExporterE2ETests()
    {
        _mockServer = WireMockServer.Start();
        _consoleOutput = new StringWriter();
        _originalConsoleOut = Console.Out;
        Console.SetOut(_consoleOutput);
    }

    [Fact]
    public async Task Program_WithCsvProviderAndConsoleExporter_ShouldPrintExchangeRatesToConsole()
    {
        // Arrange
        var csvContent = @"03 Nov 2025 #213
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|13.834
EMU|euro|1|EUR|24.340
Thailand|baht|100|THB|65.166
Turkey|lira|100|TRY|50.286
United Kingdom|pound|1|GBP|27.772
USA|dollar|1|USD|21.142";

        _mockServer
            .Given(Request.Create().WithPath("/daily.txt").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "text/plain")
                .WithBody(csvContent));

        Environment.SetEnvironmentVariable("PROVIDER_TYPE", "Csv");
        Environment.SetEnvironmentVariable("EXPORTER_TYPE", "Console");
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", $"{_mockServer.Url}/daily.txt");
        Environment.SetEnvironmentVariable("LOG_LEVEL", "Information");
        Environment.SetEnvironmentVariable("CURRENCIES", "AUD,EUR,GBP");

        // Act
        await Program.Main();

        // Assert
        // Assert
        var output = _consoleOutput.ToString();
        Assert.Contains("AUD/CZK", output);
        Assert.Contains("13", output);
        Assert.Contains("EUR/CZK", output);
        Assert.Contains("24", output);
        Assert.Contains("GBP/CZK", output);
        Assert.Contains("27", output);
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOut);
        _consoleOutput.Dispose();
        _mockServer.Stop();
        _mockServer.Dispose();

        Environment.SetEnvironmentVariable("RATE_PROVIDER_TYPE", null);
        Environment.SetEnvironmentVariable("RATE_EXPORTER_TYPE", null);
        Environment.SetEnvironmentVariable("DAILY_RATE_URL", null);
        Environment.SetEnvironmentVariable("LOG_LEVEL", null);
        Environment.SetEnvironmentVariable("CURRENCIES", null);
    }
}