using ExchangeRateProvider.Infrastructure.ExchangeRateProviders;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Infrastructure.Tests.ExchangeRateProviders;

public class HttpClientBuilderExtensionsTests
{
    private readonly IServiceCollection _services;
    private readonly IHttpClientBuilder _httpClientBuilder;

    public HttpClientBuilderExtensionsTests()
    {
        _services = new ServiceCollection();
        _httpClientBuilder = _services.AddHttpClient("test-client");
    }

    [Fact]
    public void AddResilience_WithValidName_DoesNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => _httpClientBuilder.AddResilience("test-resilience"));
        
        Assert.Null(exception);
    }

    [Fact]
    public void AddResilience_ReturnsHttpResiliencePipelineBuilder()
    {
        // Act
        var result = _httpClientBuilder.AddResilience("test-resilience");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void AddResilience_RegistersResilienceHandler()
    {
        // Act
        _httpClientBuilder.AddResilience("test-resilience");

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        Assert.NotNull(httpClientFactory);
        
        // Verify that the client can be created without throwing
        var exception = Record.Exception(() => httpClientFactory.CreateClient("test-client"));
        Assert.Null(exception);
    }

    [Fact]
    public void AddResilience_WithEmptyName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _httpClientBuilder.AddResilience(""));
    }

    [Fact]
    public void AddResilience_WithNullName_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _httpClientBuilder.AddResilience(null!));
    }

    [Fact]
    public void AddResilience_CanBeCalledMultipleTimes()
    {
        // Act & Assert
        var exception = Record.Exception(() =>
        {
            _httpClientBuilder.AddResilience("resilience-1");
            _httpClientBuilder.AddResilience("resilience-2");
        });
        
        Assert.Null(exception);
    }

    [Fact]
    public void AddResilience_CreatesHttpClientSuccessfully()
    {
        // Arrange
        _httpClientBuilder.AddResilience("test-resilience");
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act
        var httpClient = httpClientFactory.CreateClient("test-client");

        // Assert
        Assert.NotNull(httpClient);
        // Note: Polly timeout is handled by the resilience pipeline, not HttpClient.Timeout
        // HttpClient.Timeout remains at its default value while Polly handles the actual timeout
        Assert.True(httpClient.Timeout > TimeSpan.Zero);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("my-resilience-handler")]
    [InlineData("cnb-api-resilience")]
    public void AddResilience_WithVariousNames_DoesNotThrow(string handlerName)
    {
        // Act & Assert
        var exception = Record.Exception(() => _httpClientBuilder.AddResilience(handlerName));
        
        Assert.Null(exception);
    }
}