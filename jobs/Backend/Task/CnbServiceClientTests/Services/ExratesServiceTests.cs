using System.Net;
using CnbServiceClient.Models;
using CnbServiceClient.Services;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using NUnit.Framework;

namespace CnbServiceClientTests.Services
{
    public class ExratesServiceTests
	{
		private AutoMocker _autoMocker;
		private Mock<HttpMessageHandler> _httpMessageHandlerMock;

		[SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

            _httpMessageHandlerMock = _autoMocker.GetMock<HttpMessageHandler>();
		}

		[Test]
		public async Task GetExratesDaily_ReturnsExratesDaily_Succesfully()
		{
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""rates"":[{""validFor"":""2023-07-04"",""order"":128,""country"":""Austrálie"",""currency"":""dolar"",""amount"":1,""currencyCode"":""AUD"",""rate"":14.544}]}"),
            };

            _httpMessageHandlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost");

            var sut = new ExratesService(httpClient);

			// Act
			var result = await sut.GetExratesDailyAsync();

			// Assert
			result.Should().BeAssignableTo<IEnumerable<Exrate>>();
            result.Count().Should().Be(1);
		}

        [Test]
        public async Task GetExratesDaily_ThrowsException_WhenResponseIsNotSuccess()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            };

            _httpMessageHandlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost");

            var sut = new ExratesService(httpClient);

            // Act
            Func<Task> func = async () => await sut.GetExratesDailyAsync();

            // Assert
            func.Should().ThrowAsync<Exception>();
        }
    }
}

