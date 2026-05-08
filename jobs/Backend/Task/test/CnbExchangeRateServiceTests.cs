using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class CnbExchangeRateServiceTests
{
    private Mock<ILogger<CnbExchangeRateService>> CreateLoggerMock()
    {
        return new Mock<ILogger<CnbExchangeRateService>>();
    }

    private Mock<ILogger<CnbResponseParser>> CreateParserLoggerMock()
    {
        return new Mock<ILogger<CnbResponseParser>>();
    }

    private ICnbResponseParser CreateParser()
    {
        return new CnbResponseParser(CreateParserLoggerMock().Object);
    }

    private MockHttpMessageHandler CreateMockHttp()
    {
        return new MockHttpMessageHandler();
    }

    private IOptions<CnbApiOptions> CreateOptions(string baseUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml", int timeoutSeconds = 30)
    {
        var options = new CnbApiOptions
        {
            BaseUrl = baseUrl,
            TimeoutSeconds = timeoutSeconds,
            RetryCount = 3,
            CircuitBreakerFailureThreshold = 5,
            CircuitBreakerDurationSeconds = 30
        };
        return Options.Create(options);
    }

    [Fact]
    public async Task GetDailyRatesAsync_EmptyXml_ReturnsEmptyDictionary()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        var today = DateTime.Today;
        var xmlResponse = $@"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy datum=""{today:dd.MM.yyyy}""><tabulka></tabulka></kurzy>";

        mockHttp
            .When(HttpMethod.Get, "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml*")
            .Respond("application/xml", xmlResponse);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act
        var result = await service.GetDailyRatesAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDailyRatesAsync_DateMismatch_ReturnsRates()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        var requestedDate = DateTime.Today;
        var responseDate = requestedDate.AddDays(-1);
        // Use comma for decimal separator (Czech format)
        var xmlResponse = $@"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy datum=""{responseDate:dd.MM.yyyy}""><tabulka><radek kod=""USD"" mena=""dollar"" mnozstvi=""1"" kurz=""21,048"" zeme=""USA""/></tabulka></kurzy>";

        mockHttp
            .When(HttpMethod.Get, $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml?date={requestedDate:dd.MM.yyyy}")
            .Respond("application/xml", xmlResponse);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act
        var result = await service.GetDailyRatesAsync(requestedDate);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
    }

    [Fact]
    public async Task GetDailyRatesAsync_MissingDateAttribute_ReturnsRates()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        // Use comma for decimal separator (Czech format)
        var xmlResponse = @"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy><tabulka><radek kod=""USD"" mena=""dollar"" mnozstvi=""1"" kurz=""21,048"" zeme=""USA""/></tabulka></kurzy>";

        mockHttp
            .When(HttpMethod.Get, "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml*")
            .Respond("application/xml", xmlResponse);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act
        var result = await service.GetDailyRatesAsync();

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
    }

    [Fact]
    public async Task GetDailyRatesAsync_InvalidRootElement_ThrowsException()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        var xmlResponse = @"<?xml version=""1.0"" encoding=""UTF-8""?><invalid-root></invalid-root>";

        mockHttp
            .When(HttpMethod.Get, "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml*")
            .Respond("application/xml", xmlResponse);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act & Assert
        await Assert.ThrowsAsync<CnbApiException>(() => service.GetDailyRatesAsync());
    }

    [Fact]
    public async Task GetDailyRatesAsync_HttpError_ThrowsException()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        mockHttp
            .When(HttpMethod.Get, "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml*")
            .Respond(System.Net.HttpStatusCode.InternalServerError);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act & Assert
        await Assert.ThrowsAsync<CnbApiException>(() => service.GetDailyRatesAsync());
    }

    [Fact]
    public async Task GetDailyRatesAsync_InvalidXml_ThrowsException()
    {
        // Arrange
        var mockHttp = CreateMockHttp();
        var loggerMock = CreateLoggerMock();
        var invalidXml = "This is not valid XML";

        mockHttp
            .When(HttpMethod.Get, "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml*")
            .Respond("application/xml", invalidXml);

        var httpClient = mockHttp.ToHttpClient();
        var options = CreateOptions();
        var parser = CreateParser();
        var service = new CnbExchangeRateService(httpClient, loggerMock.Object, options, parser);

        // Act & Assert
        await Assert.ThrowsAsync<CnbApiException>(() => service.GetDailyRatesAsync());
    }
}
