using System.Net;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Decorator;
using Moq;
using RichardSzalay.MockHttp;

namespace TestExchangeRateUpdater.Decorator
{
    public class APICallTest
    {
        [Fact]
        public async Task Load_ReturnsTrue_WhenApiReturnsSuccessAndWrapperReturnsTrue()
        {
            // Arrange
            string expectedContent = "test response";
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("*").Respond("text/plain", expectedContent);

            HttpClient httpClient = new(mockHttp);

            Mock<ILoadRates> wrapperMock = new();
            wrapperMock.Setup(w => w.Load(expectedContent)).ReturnsAsync(true);

            APICall apiCall = new(wrapperMock.Object, httpClient);

            // Act
            bool result = await apiCall.Load("01.01.2024");

            // Assert
            Assert.True(result);
            wrapperMock.Verify(w => w.Load(expectedContent), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Fail")]
        [InlineData("Test")]
        [InlineData("01/01/2024")]
        [InlineData("21/01/2024")]
        public async Task Load_Regex_IsNOTMatch(string input)
        {
            // Arrange
            string expectedContent = "test response";
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("*").Respond("text/plain", expectedContent);

            HttpClient httpClient = new(mockHttp);

            Mock<ILoadRates> wrapperMock = new();
            wrapperMock.Setup(w => w.Load(expectedContent)).ReturnsAsync(true);

            APICall apiCall = new(wrapperMock.Object, httpClient);

            // Act
            bool result = await apiCall.Load(input);

            // Assert
            Assert.True(result);
            wrapperMock.Verify(w => w.Load(expectedContent), Times.Once);
        }

        [Fact]
        public async Task Load_ThrowsHttpRequestException()
        {
            // Arrange
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("*").Throw(new HttpRequestException());

            HttpClient httpClient = new(mockHttp);

            Mock<ILoadRates> wrapperMock = new();

            APICall apiCall = new(wrapperMock.Object, httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => apiCall.Load(""));
        }

        [Fact]
        public async Task Load_IsUnSuccessStateCode()
        {
            // Arrange
            MockHttpMessageHandler mockHttp = new();
            mockHttp.When("*").Respond(HttpStatusCode.NotFound);

            HttpClient httpClient = new(mockHttp);

            Mock<ILoadRates> wrapperMock = new();

            APICall apiCall = new(wrapperMock.Object, httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => apiCall.Load(""));
        }
    }
}
