using System.Net;
using System.Text;
using System.Text.Json;
using ExchangeRateUpdater.Infrastructure.Common;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Infrastructure.Tests.Common;

public class RestClientTests
{
    [Theory]
    [MemberData(nameof(ResponseModels))]
    public async Task GetAsync_When_requested_with_model_Then_model_is_deserialized_as_expected(RestClientTestModel? expectedModel)
    {
        //Arrange
        const string uri = "http://fakeuri.com";
        var testModelStream = ConvertToStream(expectedModel);
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(testModelStream)
        };
        var sut = GetRestClient(apiResponse);
        
        //Act
        var result = await sut.GetAsync<RestClientTestModel?>(uri);
        
        //Assert
        Assert.Equal(expectedModel, result);
    }
    
    [Fact]
    public async Task GetAsync_When_requested_with_string_Then_string_is_returned()
    {
        //Arrange
        const string uri = "http://fakeuri.com";
        const string expectedResponseString = "ExpectedResponseString";
        var apiResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(expectedResponseString)
        };
        var sut = GetRestClient(apiResponse);
        
        //Act
        var result = await sut.GetAsync<string?>(uri);
        
        //Assert
        Assert.Equal(expectedResponseString, result);
    }

    public static TheoryData<RestClientTestModel?> ResponseModels = new()
    {
        new RestClientTestModel { Id = 33, Name = "TestModel", Date = DateTime.UtcNow },
        null
    };
    
    public readonly record struct RestClientTestModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public DateTime Date { get; init; }
    }

    private static MemoryStream ConvertToStream(RestClientTestModel? model)
    {
        var serializedModel = JsonSerializer.Serialize(model);
        var serializedModelBytes = Encoding.UTF8.GetBytes(serializedModel);
        var base64SerializedModel = Convert.ToBase64String(serializedModelBytes);
        var stream = new MemoryStream(Convert.FromBase64String(base64SerializedModel));
        return stream;
    }
    
    private static RestClient GetRestClient(HttpResponseMessage messageToReturn)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(messageToReturn);
        var httpClientMock = new HttpClient(httpMessageHandlerMock.Object);
        return new(httpClientMock);
    }
}