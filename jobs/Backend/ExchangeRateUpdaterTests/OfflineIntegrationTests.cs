using ExchangeRateUpdater;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Moq;
using Ninject;

namespace ExchangeRateUpdaterTests
{
	[TestClass]
	public class OfflineIntegrationTests
	{
		private Mock<IWebRequestService> mockWebRequestService;
		private Mock<ILogger> mockLogger;

		private IKernel kernel;
		private Program program;

		[TestInitialize]
		public void TestInitialize()
		{
			kernel = new StandardKernel(new DependencyInjectionConfiguration());

			mockWebRequestService = new Mock<IWebRequestService>();
			mockLogger = new Mock<ILogger>(MockBehavior.Strict);

			kernel.Rebind<IWebRequestService>().ToConstant(mockWebRequestService.Object);
			kernel.Rebind<ILogger>().ToConstant(mockLogger.Object);

			program = kernel.Get<Program>();
		}

		[TestMethod]
		public async Task ExchangeRateUpdater_SuccessFlow()
		{
			var actualInfoMessages = new List<string>();

			mockWebRequestService
				.Setup(m => m.GetAsStringAsync(It.Is<Uri>(uri => uri == ExchangeRateProvider.CnbExchangeRateDataUri)))
				.ReturnsAsync(string.Join(
					Environment.NewLine,
					"16 Sep 2022 #181",
					"Country|Currency|Amount|Code|Rate",
					"Australia|dollar|1|AUD|16.446",
					"Brazil|real|1|BRL|4.683",
					"Bulgaria|lev|1|BGN|12.566",
					"Canada|dollar|1|CAD|18.518",
					"China|renminbi|1|CNY|3.510",
					"Croatia|kuna|1|HRK|3.256",
					"Denmark|krone|1|DKK|3.294",
					"EMU|euro|1|EUR|24.495",
					"Hongkong|dollar|1|HKD|3.135",
					"Hungary|forint|100|HUF|6.063",
					"Iceland|krona|100|ISK|17.711",
					"IMF|SDR|1|XDR|31.862",
					"India|rupee|100|INR|30.862",
					"Indonesia|rupiah|1000|IDR|1.646",
					"Israel|new shekel|1|ILS|7.148",
					"Japan|yen|100|JPY|17.183",
					"Malaysia|ringgit|1|MYR|5.426",
					"Mexico|peso|1|MXN|1.224",
					"New Zealand|dollar|1|NZD|14.653",
					"Norway|krone|1|NOK|2.401",
					"Philippines|peso|100|PHP|42.886",
					"Poland|zloty|1|PLN|5.196",
					"Romania|leu|1|RON|4.976",
					"Singapore|dollar|1|SGD|17.464",
					"South Africa|rand|1|ZAR|1.392",
					"South Korea|won|100|KRW|1.770",
					"Sweden|krona|1|SEK|2.277",
					"Switzerland|franc|1|CHF|25.567",
					"Thailand|baht|100|THB|66.551",
					"Turkey|lira|1|TRY|1.346",
					"United Kingdom|pound|1|GBP|28.030",
					"USA|dollar|1|USD|24.606")
				);
			mockLogger
				.Setup(m => m.LogInfoAsync(It.IsAny<string>()))
				.Callback<string>(actualInfoMessages.Add)
				.Returns(Task.CompletedTask);

			await program.RunAsync().ConfigureAwait(false);

			actualInfoMessages.Should()
				.HaveCount(6).And
				.ContainInOrder(
					"Successfully retrieved 5 exchange rates:",
					$"USD/CZK={24.606M}",
					$"EUR/CZK={24.495M}",
					$"JPY/CZK={0.17183M}",
					$"THB/CZK={0.66551M}",
					$"TRY/CZK={1.346M}"
				);
		}

		[TestMethod]
		public async Task ExchangeRateUpdater_FailureFlow()
		{
			var actualErrorMessages = new List<string>();
			var exceptionMessage = "ExchangeRateProvider exception";

			mockWebRequestService
				.Setup(m => m.GetAsStringAsync(It.Is<Uri>(uri => uri == ExchangeRateProvider.CnbExchangeRateDataUri)))
				.ThrowsAsync(new Exception(exceptionMessage));
			mockLogger
				.Setup(m => m.LogErrorAsync(It.IsAny<string>()))
				.Callback<string>(actualErrorMessages.Add)
				.Returns(Task.CompletedTask);

			await program.RunAsync().ConfigureAwait(false);

			actualErrorMessages.Should().BeEquivalentTo($"Could not retrieve exchange rates: '{exceptionMessage}'.");
		}
	}
}
