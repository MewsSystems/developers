using System.Net;
using System.Text;
using ExchangeRatesService.Models;
using ExchangeRatesService.Providers;
using ExchangeRatesService.Providers.Interfaces;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Moq.Protected;

namespace ExchangeRatesTest;

[TestFixture]
public class ExchangeRateProviderTest
{
    private IRatesProvider _service;
    private Mock<HttpClient> _httpClient;
    private Mock<HttpMessageHandler> _httpMessageHandler;
    
    private const string fakeBaseAddress = "https://www.fake.com";
    
    private FakeTimeProvider _timeProvider;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _httpMessageHandler = new Mock<HttpMessageHandler>();
        var httpConcreteClient = new HttpClient(_httpMessageHandler.Object);
        httpConcreteClient.BaseAddress = new Uri(fakeBaseAddress);
        _service = new ExchangeRateProvider(httpConcreteClient);
        
        _timeProvider = new FakeTimeProvider(DateTimeOffset.Parse("2024-02-05T14:00:00.000Z"));
    }

    [Test]
    public async Task GetExchangeRates_ShoudlReturnACountOf2Rates()
    {
        //Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        var fakeJsonResponse =
            "{\n\t\"rates\": [\n\t\t{\n\t\t\t\"amount\": 1,\n\t\t\t\"country\": \"Australia\",\n\t\t\t\"currency\": \"dollar\",\n\t\t\t\"currencyCode\": \"AUD\",\n\t\t\t\"order\": 94," +
            "\n\t\t\t\"rate\": 15.858,\n\t\t\t\"validFor\": \"2019-05-17\"\n\t\t},\n\t\t{\n\t\t\t\"amount\": 100,\n\t\t\t\"country\": \"Japan\",\n\t\t\t\"currency\": \"yen\",\n\t\t\t\"currencyCode\": \"JPY\",\n\t\t\t\"order\": 94," +
            "\n\t\t\t\"rate\": 21.045,\n\t\t\t\"validFor\": \"2019-05-17\"\n\t\t},\n\t\t{\n\t\t\t\"validFor\": \"2019-05-17\",\n\t\t\t\"order\": 94,\n\t\t\t\"country\": \"Russia\",\n\t\t\t\"currency\": \"rouble\",\n\t\t\t\"amount\": 100,\n\t\t\t\"currencyCode\": \"RUB\"," +
            "\n\t\t\t\"rate\": 35.668\n\t\t}\n\t]\n}";

        _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeJsonResponse, Encoding.UTF8, "application/json")
            })
            .Verifiable();

        //Act
        var rates = _service.GetRatesAsync(currencies).GetAsyncEnumerator();
        var count = 0;
        try
        {
            while (await rates.MoveNextAsync()) Console.Write($"{count++}");
        }
        finally
        {
            if (rates != null) rates.DisposeAsync();
        }

        //Assert
        Assert.That(count, Is.EqualTo(2));
    }
    
    
    [Test]
    public async Task CalculateReverseRate_ShouldBe4()
    {
        //Arrange
        var currencies = new[]
        {
            new Currency("EUR")
        };

        var fakeJsonResponse =
            "{\n\t\"rates\": [\n\t\t{\n\t\t\t\"validFor\": \"2024-02-12\",\n\t\t\t\"order\": 30,\n\t\t\t\"country\": \"EMU\",\n\t\t\t\"currency\": \"euro\"," +
            "\n\t\t\t\"amount\": 1,\n\t\t\t\"currencyCode\": \"EUR\",\n\t\t\t\"rate\": 0.25\n\t\t}\n\t]\n}";

        _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeJsonResponse, Encoding.UTF8, "application/json")
            })
            .Verifiable();

        //Act
        var rates = _service.GetRatesReverseAsync(currencies, 10).GetAsyncEnumerator();

        var value = 0m;
        try
        {
            while (await rates.MoveNextAsync())
            {
                value = rates.Current.Value;
                Console.Write($"{rates.Current.Value}");
            }
        }
        finally
        {
            if (rates != null) rates.DisposeAsync();
        }

        //Assert
        Assert.That(value, Is.EqualTo(4m));
    }
    
    [Test]
    public async Task CalculateReverseRate1By100Rate_ShouldBe400000()
    {
        //Arrange
        var currencies = new[]
        {
            new Currency("ISK")
        };

        var fakeJsonResponse =
            "{\n\t\"rates\": [\n\t\t{\n\t\t\t\"validFor\": \"2024-02-12\",\n\t\t\t\"order\": 30,\n\t\t\t\"country\": " +
            "\"Iceland\",\n\t\t\t\"currency\": \"krona\",\n\t\t\t\"amount\": 100,\n\t\t\t\"currencyCode\": \"ISK\",\n\t\t\t\"rate\": 0.000025\n\t\t}\n\t]\n}";

        _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeJsonResponse, Encoding.UTF8, "application/json")
            })
            .Verifiable();

        //Act
        var rates = _service.GetRatesReverseAsync(currencies, 10).GetAsyncEnumerator();

        var value = 0m;
        try
        {
            while (await rates.MoveNextAsync())
            {
                value = rates.Current.Value;
                Console.Write($"{rates.Current.Value}");
            }
        }
        finally
        {
            if (rates != null) rates.DisposeAsync();
        }

        //Assert
        Assert.That(value, Is.EqualTo(40000m));
    }
}