using ExchangeRateUpdater.Client;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdaterTests.ClientTests
{
    public class RetryPolicyTests
    {
		readonly HttpClient _httpClient;
		readonly Mock<HttpMessageHandler> _httpMessageHandler;
		Mock<ILogger<RetryPolicy>> _loggerMock;

		public RetryPolicyTests()
        {
			_httpMessageHandler = new Mock<HttpMessageHandler>();
			_loggerMock = new Mock<ILogger<RetryPolicy>>();
			_httpClient = new HttpClient(_httpMessageHandler.Object)
			{
				BaseAddress = new Uri("https://api.cnb.cz")
			};
		}

		[SetUp]
		public void SetUp()
		{
			_loggerMock = new Mock<ILogger<RetryPolicy>>();
		}

		[Test]
		public void GivenASuccesfullRequest_WhenExecuteGetRequestWithRetryCalled_ThenResponseIsReturned()
		{
			var response = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.OK,
			};

			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response)
				.Verifiable(Times.Once);

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 2).Result;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.IsSuccessStatusCode);
			_httpMessageHandler.VerifyAll();
		}

		[Test]
		public void GivenASuccesfullRequest_WhenExecuteGetRequestWithRetryCalled_ThenRequestIsLogged()
		{
			var response = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.OK,
			};

			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 2).Result;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.IsSuccessStatusCode);
			_loggerMock.Verify(x => x.Log(
				It.Is<LogLevel>(l => l == LogLevel.Information),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Successful GET request to {_httpClient.BaseAddress}")),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
		}

		[Test]
		public void GivenAFailedRequest_WhenExecuteGetRequestWithRetryCalled_ThenSeveralRequestsAreMade()
		{
			var response = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.BadRequest,
			};

			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response)
				.Verifiable(Times.Exactly(3));

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 0).Result;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
			_httpMessageHandler.VerifyAll();
		}

		[Test]
		public void GivenAFailedRequest_WhenExecuteGetRequestWithRetryCalled_ThenFailedRequestsAreLogged()
		{
			var response = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.BadRequest,
			};

			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 0).Result;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.BadRequest);
			
			_loggerMock.Verify(x => x.Log(
				It.Is<LogLevel>(l => l == LogLevel.Warning),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Failed GET request to {_httpClient.BaseAddress}")),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(3));
		}

		[Test]
		public void GivenAnExcptionIsThrown_WhenExecuteGetRequestWithRetryCalled_ThenSeveralRequestsAreMade()
		{
			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ThrowsAsync(new InvalidOperationException())
				.Verifiable(Times.Exactly(3));

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 0).Result;

			Assert.IsNull(result);
			_httpMessageHandler.VerifyAll();
		}

		[Test]
		public void GivenAnExcptionIsThrown_WhenExecuteGetRequestWithRetryCalled_ThenExceptionIsLogged()
		{
			var ex = new InvalidOperationException("Exception from http client");

			var retryPolicy = new RetryPolicy(_loggerMock.Object);
			_httpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ThrowsAsync(ex);

			var result = retryPolicy.ExecuteGetRequestWithRetry(_httpClient, string.Empty, 3, 0).Result;

			Assert.IsNull(result);

			_loggerMock.Verify(x => x.Log(
				It.Is<LogLevel>(l => l == LogLevel.Error),
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Exception thrown: {ex.GetType()}: {ex.Message}")),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(3));
		}
	}
}
